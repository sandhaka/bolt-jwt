using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Users
{
    public interface IUserQueries
    {
        /// <summary>
        /// Return a user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User info</returns>
        /// <exception cref="KeyNotFoundException">User not found</exception>
        Task<object> GetAsync(int id);

        /// <summary>
        /// Retrieve users list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged users data</returns>
        Task<PagedData<object>> GetAsync(PageQuery query);

        /// <summary>
        /// Retrieve the user authorizations list
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Authorizations list</returns>
        Task<dynamic> GetAuthAsync(int id);

        /// <summary>
        /// Return user roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Roles</returns>
        Task<dynamic> GetRolesAsync(int userId);
    }
}