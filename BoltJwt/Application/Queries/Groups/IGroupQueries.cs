using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Groups
{
    public interface IGroupQueries
    {
        /// <summary>
        /// Retrieve groups list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged groups data</returns>
        Task<PagedData<object>> GetAsync(PageQuery query);

        /// <summary>
        /// Return group roles
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <returns>Roles</returns>
        Task<dynamic> GetRolesAsync(int groupId);
    }
}