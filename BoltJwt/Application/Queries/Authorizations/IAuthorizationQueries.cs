using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Authorizations
{
    public interface IAuthorizationQueries
    {
        /// <summary>
        /// Retrieve the authorizations definition list
        /// </summary>
        /// <returns>Authorizations list</returns>
        Task<IEnumerable<dynamic>> GetAsync();

        /// <summary>
        /// Retrieve the authorizations definition list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged authorizations data</returns>
        Task<PagedData<dynamic>> GetAsync(PageQuery query);

        /// <summary>
        /// Return authorization usage
        /// </summary>
        /// <param name="id">Authorization id</param>
        /// <returns>Lists of user and role names</returns>
        Task<dynamic> GetUsageAsync(int id);
    }
}