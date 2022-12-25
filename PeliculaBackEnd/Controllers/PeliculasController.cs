using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PeliculaBackEnd.DTOs;
using PeliculaBackEnd.Entidades;
using PeliculaBackEnd.Utilidades;

namespace PeliculaBackEnd.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy ="EsAdmin")]
    public class PeliculasController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";
        public PeliculasController(ApplicationDbContext context,
                                    IMapper mapper, IAlmacenadorArchivos almacenadorArchivos, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDTO>>Get(){
            var top = 6;
            var hoy = DateTime.Today;

            var proximosEstrenos = await context.peliculas.Where(x => x.FechaLanzamiento > hoy).
                OrderBy(x => x.FechaLanzamiento).Take(top).ToListAsync();

            var enCines = await context.peliculas.Where(x => x.enCines).
                OrderBy(x => x.FechaLanzamiento).Take(top).ToListAsync();

            var resultado = new LandingPageDTO();
            
                resultado.proximosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
                resultado.enCines = mapper.Map<List<PeliculaDTO>>(enCines);
            return resultado;
        }

        [HttpGet("buscar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDTO>>> GetAll()
        {

            var pelicula = await context.peliculas.ToListAsync();
            mapper.Map<List<PeliculaDTO>>(pelicula);
            return mapper.Map<List<PeliculaDTO>>(pelicula);
           
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula =   context.peliculas.Include(x => x.peliculasGeneros).ThenInclude(x => x.genero)
                .Include(x=> x.peliculasActores).ThenInclude(x=>x.actor)
                .Include(x=> x.peliculasCines).ThenInclude(x=>x.cine)
                .FirstOrDefault(x=>x.id==id);

            if (pelicula == null)
            {
                return NotFound();
            }

            var promedioVoto = 0.0;
            var usuarioVoto = 0;

            if(await context.Ratings.AnyAsync(x=>x.peliculaId == id))
            {
                promedioVoto = await context.Ratings.Where(x => x.peliculaId == id)
                    .AverageAsync(x => x.puntuacion);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var usuario = await userManager.FindByEmailAsync(email);
                    var usuarioId = usuario.Id;
                    var ratingDB  = await context.Ratings
                    .FirstOrDefaultAsync(x => x.usuarioId == usuarioId && x.peliculaId == id);

                    if(ratingDB != null)
                    {
                        usuarioVoto = ratingDB.puntuacion;
                    }
                }
                
            }

            var dto = mapper.Map<PeliculaDTO>(pelicula);
            dto.votoUsuario = usuarioVoto;
            dto.promedioVoto = promedioVoto;
            dto.actores = dto.actores.OrderBy(x => x.orden).ToList();

            return dto;
        }


        [HttpPost]

        public async Task<ActionResult<int>> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.poster != null)
            {
                pelicula.poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.poster);
            }
            EscribirOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            

            return pelicula.id;


        }

        [HttpGet("PostGet")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDTO>>(cines);
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDTO() { cines = cinesDTO, generos = generosDTO };
        }
        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            var peliculasQueryable = context.peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltrarDTO.titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.titulo.Contains(peliculasFiltrarDTO.titulo));
            }

            if (peliculasFiltrarDTO.enCines)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.enCines);
            }

            if (peliculasFiltrarDTO.proximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaLanzamiento > hoy);
            }

            if (peliculasFiltrarDTO.generoId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.peliculasGeneros.Select(y => y.generoId)
                    .Contains(peliculasFiltrarDTO.generoId));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculasQueryable);

            var peliculas = await peliculasQueryable.paginar(peliculasFiltrarDTO.paginacionDTO).ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }


        [HttpGet("PutGet/{id:int}")]

        public async Task<ActionResult<PeliculasPutGetDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult)
            {
                return NotFound();
            }
            var pelicula = peliculaActionResult.Value;
            var generosSeleccionadosIds = pelicula.generos.Select(x => x.id).ToList();
            var generosNoSeleccionados = await context.Generos.Where(x => !generosSeleccionadosIds.Contains(x.id)).ToListAsync();

            var cinesSeleccionadosIds = pelicula.cines.Select(x => x.id).ToList();
            var cinesNoSeleccionados = await context.Cines.Where(x => !cinesSeleccionadosIds.Contains(x.id)).ToListAsync();


            var generosNoSeleccionadosDTO = mapper.Map<List<GeneroDTO>>(generosNoSeleccionados);
            var cinesNoSeleccionadosDTO = mapper.Map<List<CineDTO>>(cinesNoSeleccionados);

            var respuesta = new PeliculasPutGetDTO();
            respuesta.pelicula = pelicula;
            respuesta.generosSeleccionados = pelicula.generos;
            respuesta.generosNoSeleccionados = generosNoSeleccionadosDTO;
            respuesta.cinesSeleccionados = pelicula.cines;
            respuesta.cinesNoSeleccionados = cinesNoSeleccionadosDTO;
            respuesta.actores = pelicula.actores;

            return respuesta;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = await context.peliculas.Include(x => x.peliculasActores).
                Include(x => x.peliculasGeneros).Include(x => x.peliculasCines).FirstOrDefaultAsync( x=>x.id ==id);

            if(pelicula == null)
            {
                return NotFound();
            }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);
            if(peliculaCreacionDTO.poster != null)
            {
                pelicula.poster = await almacenadorArchivos.EditarArchivo(contenedor, peliculaCreacionDTO.poster, pelicula.poster);

            }

            EscribirOrdenActores(pelicula);

            await context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult>Delete(int id)
        {
            var pelicula = await context.peliculas.FirstOrDefaultAsync(x => x.id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();

            await almacenadorArchivos.BorrarArchivo(pelicula.poster, contenedor);

            return NoContent();
        }

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if(pelicula.peliculasActores != null)
            {
                for(int i =0; i<pelicula.peliculasActores.Count; i++)
                {
                    pelicula.peliculasActores[i].Orden = i;
                }
            }
        }



    }
}
