
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.Entidades
{
    public class Pelicula
    {

        public int id { get; set; }

        [Required]
        [StringLength(maximumLength:300)]
        public string titulo { get; set; }

        public string resumen { get; set; }
        public string trailer { get; set; }

        public bool enCines { get; set; }

        public DateTime? FechaLanzamiento { get; set; }

        public string poster { get; set; }

        public  List<PeliculasActores> peliculasActores { get; set; }
   
        public List<PeliculasGeneros> peliculasGeneros { get; set; }

        public List<PeliculasCines> peliculasCines { get; set; }  
    
    
    
    }
}
