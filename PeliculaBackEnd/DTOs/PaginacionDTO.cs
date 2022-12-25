namespace PeliculaBackEnd.DTOs
{
    public class PaginacionDTO
    {

        public int pagina { get; set; } = 1;

        public int recordsPorPagina = 10;
        private int cantidadMaximaRecordsPorPagina = 50;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina=(value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina: value;
            }
        }
    }
}
