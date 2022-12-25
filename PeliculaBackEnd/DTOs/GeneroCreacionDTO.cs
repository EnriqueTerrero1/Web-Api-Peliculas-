using PeliculaBackEnd.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.DTOs
{
    public class GeneroCreacionDTO
    {

        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }
    }
}
