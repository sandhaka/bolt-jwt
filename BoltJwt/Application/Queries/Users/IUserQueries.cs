using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries.Users
{
    public interface IUserQueries
    {
        Task<object> GetAsync(int id);
        Task<PagedData<object>> GetAsync(PageQuery query);
        Task<dynamic> GetAuthAsync(int id);
    }
}