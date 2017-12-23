using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Authorizations
{
    public interface IAuthorizationQueries
    {
        Task<IEnumerable<dynamic>> GetAsync();
        Task<PagedData<dynamic>> GetAsync(PageQuery query);
        Task<dynamic> GetUsageAsync(int id);
    }
}