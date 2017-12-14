using System.Threading.Tasks;

namespace BoltJwt.Application.Queries.Roles
{
    public interface IRoleQueries
    {
        Task<object> GetAsync();
    }
}