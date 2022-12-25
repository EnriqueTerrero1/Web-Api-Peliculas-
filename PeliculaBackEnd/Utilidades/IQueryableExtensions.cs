using PeliculaBackEnd.DTOs;

namespace PeliculaBackEnd.Utilidades
{
    public  static class  IQueryableExtensions
    {

        public static IQueryable<T> paginar<T> (this IQueryable<T> queryable,PaginacionDTO paginacionDTO)
        {
            return queryable.Skip((paginacionDTO.pagina - 1) * paginacionDTO.RecordsPorPagina).Take(paginacionDTO.RecordsPorPagina);
        }
    }
}
