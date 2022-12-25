using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.Entidades
{
    public class PeliculasActores
    {

        public int peliculaId { get; set; }

        public int actorId { get; set; }

        public Pelicula pelicula { get; set; }

        public Actor actor { get; set; }

        [StringLength(maximumLength:100)]
        public string personaje { get; set; }

        public int Orden { get; set; }

    }
}
