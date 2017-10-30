using System.Threading.Tasks;
using BoltJwt.Model;
using BoltJwt.Model.Abstractions;

namespace BoltJwt.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        public User Add(User role)
        {
            throw new System.NotImplementedException();
        }

        public void Update(User role)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}