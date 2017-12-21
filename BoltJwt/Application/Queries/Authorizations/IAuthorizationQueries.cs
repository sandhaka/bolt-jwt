using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Authorizations
{
    public interface IAuthorizationQueries
    {
        Task<PagedData<dynamic>> GetAsync(PageQuery query);
    }
}