using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.Entidades
{
    public class Rating
    {
        public int id { get; set; }

        [Range(1,5)]
        public int puntuacion { get; set; }

        public int peliculaId { get;set; }
        public Pelicula pelicula { get; set; }

        public string usuarioId { get; set; }
        public IdentityUser usuario { get; set; }

    }
}
