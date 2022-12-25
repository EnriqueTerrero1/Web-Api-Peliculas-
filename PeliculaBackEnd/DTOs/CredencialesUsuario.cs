using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.DTOs
{
    public class CredencialesUsuario
    {

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
