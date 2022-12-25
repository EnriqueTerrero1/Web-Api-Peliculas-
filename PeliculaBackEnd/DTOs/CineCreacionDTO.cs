using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.DTOs
{
    public class CineCreacionDTO
    {
        
        [StringLength(maximumLength:75)]
        public string nombre {get;set;}

        [Range(-90,90)]
        public  double latitud { get;set;}
        [Range(-180, 180)]
        public double longitud { get; set; }
    }
}
