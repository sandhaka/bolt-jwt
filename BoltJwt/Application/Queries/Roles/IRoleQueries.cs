using System.Collections.Generic;
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

        /// <summary>
        /// Return a role by id
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        /// <exception cref="KeyNotFoundException">Role not found</exception>
        Task<dynamic> GetAsync(int id);

        /// <summary>
        /// Retrieve the role authorizations list
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Authorizations list</returns>
        Task<dynamic> GetAuthAsync(int id);
    }
}