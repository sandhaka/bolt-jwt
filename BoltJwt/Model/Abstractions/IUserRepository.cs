using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Model.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<ClaimsIdentity> GetIdentityAsync(DbContext dbContext, string username, string password);
    }
}