using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoltJwt.Application.Queries
{
    public interface IUserQueries
    {
        Task<object> GetUserAsync(int id);
        Task<IEnumerable<object>> GetUsersAsync();
    }
}