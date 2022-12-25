using PeliculaBackEnd.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.Entidades
{
    public class Genero
    {
        public int id { get; set; }

        [StringLength(maximumLength:50)]
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }

        
    public List<PeliculasGeneros> peliculasGeneros { get; set; } 
    }
}
