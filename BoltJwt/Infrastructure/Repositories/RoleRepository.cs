using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly IdentityContext _context;

        public RoleRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a new Role to the context
        /// </summary>
        /// <param name="item">Role</param>
        /// <returns>Role added</returns>
        public Role Add(Role item)
        {
            return _context.Roles.Add(item).Entity;
        }

        /// <summary>
        /// Mark the entity as 'modified'.
        /// The entity exists in the database and has been modified on the client.
        /// SaveChanges should send updates for it.
        /// </summary>
        /// <param name="item">Role</param>
        public void Update(Role item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Return the role by id with his properties
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        public async Task<Role> GetAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role != null)
            {
                await _context.Entry(role)
                    .Collection(i => i.GroupRoles).LoadAsync();
                await _context.Entry(role)
                    .Collection(i => i.UserRoles).LoadAsync();
                await _context.Entry(role)
                    .Collection(i => i.Authorizations).LoadAsync();
            }

            return role;
        }
    }
}