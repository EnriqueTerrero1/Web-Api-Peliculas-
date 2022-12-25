namespace PeliculaBackEnd.Entidades
{
    public class PeliculasGeneros
    {

        public int peliculaId { get; set; }
        public int generoId { get; set; }
        public Pelicula pelicula { get; set; }

        public Genero genero { get; set; }
    }
}
