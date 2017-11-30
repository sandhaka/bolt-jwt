using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;

namespace BoltJwt.Application.Queries
{
    public interface IUserQueries
    {
        Task<object> GetAsync(int id);
        Task<PagedData<object>> GetAsync(PageQuery query);
    }
}