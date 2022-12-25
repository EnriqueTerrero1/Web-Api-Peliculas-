using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.Entidades
{
    public class Cine
    {
        public int id { get; set; }

        [Required]
        [StringLength(maximumLength:75)]
        public string nombre { get; set; }

        public  Point ubicacion { get; set; }

        public List<PeliculasCines> peliculasCines { get; set; }
    }
}
