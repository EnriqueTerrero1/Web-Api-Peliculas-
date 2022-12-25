using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.DTOs
{
    public class ActorCreacionDTO
    {

       
        [Required]
        [StringLength(maximumLength:200)]
        public string nombre { get; set; }

        public string biografia { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public IFormFile? foto { get; set; }

    }
}
