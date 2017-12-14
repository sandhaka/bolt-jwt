using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoltJwt.Application.Queries.Authorizations
{
    public interface IAuthorizationQueries
    {
        Task<IEnumerable<object>> GetAsync();
    }
}