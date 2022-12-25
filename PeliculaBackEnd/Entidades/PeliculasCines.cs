namespace PeliculaBackEnd.Entidades
{
    public class PeliculasCines
    {

        public int peliculaId {get;set;}
        public int cineId { get;set;}
        public Pelicula Pelicula {get;set;}

        public Cine cine {get;set;}
    }
}
