namespace PeliculaBackEnd.DTOs
{
    public class PeliculasFiltrarDTO
    {

        public int pagina { get; set; }
        public int recordsPorPagina { get; set; }

        public PaginacionDTO paginacionDTO {
            get{
                return new PaginacionDTO()
                {
                    pagina = pagina,
                    recordsPorPagina = recordsPorPagina

                };
            } 

        }

        public string? titulo { get; set;}
        public int generoId { get; set; }
        public bool enCines { get; set; }
        public bool proximosEstrenos { get; set; }
    }
}
