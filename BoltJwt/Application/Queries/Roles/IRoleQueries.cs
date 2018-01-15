using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Roles
{
    public interface IRoleQueries
    {
        /// <summary>
        /// Retrieve roles list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged roles data</returns>
        Task<PagedData<dynamic>> GetAsync(PageQuery query);
    }
}