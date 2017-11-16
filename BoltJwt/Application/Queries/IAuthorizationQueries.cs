using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoltJwt.Application.Queries
{
    public interface IAuthorizationQueries
    {
        Task<IEnumerable<object>> GetAsync();
    }
}