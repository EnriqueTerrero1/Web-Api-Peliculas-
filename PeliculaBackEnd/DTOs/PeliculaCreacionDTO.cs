using Microsoft.AspNetCore.Mvc;
using PeliculaBackEnd.Utilidades;
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.DTOs
{
    public class PeliculaCreacionDTO
    {

        public int id { get; set; }

        [Required]
        [StringLength(maximumLength: 300)]
        public string titulo { get; set; }

        public string resumen { get; set; }
        public string trailer { get; set; }

        public bool enCines { get; set; }

        public DateTime? FechaLanzamiento { get; set; }

        public IFormFile poster { get; set; }

        [ModelBinder(BinderType =typeof(TypeBinder<List<int>>))]
        public List<int> generosIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinesIds { get; set; }

        [ModelBinder(BinderType =typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO> actores { get; set; }

    }
}
