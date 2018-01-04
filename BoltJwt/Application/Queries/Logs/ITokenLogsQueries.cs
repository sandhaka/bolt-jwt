using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Logs
{
    public interface ITokenLogsQueries
    {
        /// <summary>
        /// Retrieve generated token list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged data</returns>
        Task<PagedData<object>> GetAsync(PageQuery query);
    }
}