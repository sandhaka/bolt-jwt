using System.Threading.Tasks;

namespace BoltJwt.Application.Queries
{
    public interface IRoleQueries
    {
        Task<object> GetAsync();
    }
}