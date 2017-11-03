using System.Security.Claims;
using System.Threading.Tasks;

namespace BoltJwt.Model.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<ClaimsIdentity> GetIdentityAsync(string username, string password);
    }
}