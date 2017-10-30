using System;
using System.Threading.Tasks;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Model;
using BoltJwt.Model.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IdentityContext _context;

        public UserRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a new user to the context
        /// </summary>
        /// <param name="item">User</param>
        /// <returns>User added</returns>
        public User Add(User item)
        {
            return _context.Users.Add(item).Entity;
        }

        /// <summary>
        /// Mark the entity as 'modified'.
        /// The entity exists in the database and has been modified on the client.
        /// SaveChanges should send updates for it.
        /// </summary>
        /// <param name="item">User</param>
        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Return the user by id with his properties
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        public async Task<User> GetAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(i => i.UserGroups).LoadAsync();
                await _context.Entry(user)
                    .Collection(i => i.UserRoles).LoadAsync();
                await _context.Entry(user)
                    .Collection(i => i.Authorizations).LoadAsync();
            }

            return user;
        }
    }
}