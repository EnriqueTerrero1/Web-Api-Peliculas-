using System.ComponentModel.DataAnnotations;

namespace PeliculaBackEnd.DTOs
{
    public class ActorDTO
    {
        public int id { get; set; }

       
        public string nombre { get; set; }

        public string biografia { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public string foto { get; set; }



    }
}
