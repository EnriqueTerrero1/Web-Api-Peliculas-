using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PeliculaBackEnd.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PeliculaBackEnd
{
    public class ApplicationDbContext:IdentityDbContext
    {

        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options):base(options)
        {



        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>().HasKey(x => new { x.actorId, x.peliculaId });
            modelBuilder.Entity<PeliculasGeneros>().HasKey(x => new { x.generoId, x.peliculaId });
            modelBuilder.Entity<PeliculasCines>().HasKey(x => new { x.cineId, x.peliculaId });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }

        public DbSet<Cine>Cines { get; set; }

        public DbSet<Pelicula> peliculas { get; set; }
        public DbSet<PeliculasActores> peliculasActores { get; set;}
        public DbSet<PeliculasCines> peliculasCines { get;set; }
        public DbSet<PeliculasGeneros> peliculasGeneros { get; set; }
        
        public DbSet<Rating> Ratings { get; set; }
    
    }
}
