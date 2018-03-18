using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    /// <summary>
    /// Define role operations
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get => _context;
        }

        public RoleRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get role
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        public async Task<Role> GetAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id) ??
                   throw new EntityNotFoundException(nameof(Role));
        }

        /// <summary>
        /// Get role with authorizations
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        public async Task<Role> GetWithAuthorizationsAsync(int id)
        {
            return await _context.Roles.Include(r => r.Authorizations).FirstOrDefaultAsync(r => r.Id == id) ??
                   throw new EntityNotFoundException(nameof(Role));
        }

        /// <summary>
        /// Get roles
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Roles</returns>
        public IEnumerable<Role> Query(Func<Role, bool> query = null)
        {
            return query == null ?
                _context.Roles :
                _context.Roles.Where(query);
        }

        /// <summary>
        /// Add a new role
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>Role added</returns>
        public Role Add(Role role)
        {
            _context.Add(role).State = EntityState.Added;
            return role;
        }

        /// <summary>
        /// Mark a role as deleted
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Role not found</exception>
        public void Delete(Role role)
        {
            _context.Entry(role).State = EntityState.Deleted;
        }

        /// <summary>
        /// Check role usage
        /// </summary>
        /// <param name="role">Role</param>
        /// <exception cref="EntityInUseException"></exception>
        public void CheckUsasge(Role role)
        {
            if (_context.Groups.Any(g => g.GroupRoles.Any(r => r.RoleId == role.Id)) ||
                _context.Users.Any(u => u.UserRoles.Any(r => r.RoleId == role.Id)))
            {
                throw new EntityInUseException(role.Description);
            }
        }
    }
}