using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
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
        /// Get roles
        /// </summary>
        /// <returns>Roles</returns>
        public IEnumerable<Role> GetAll()
        {
            return _context.Roles;
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
        /// Update role description
        /// </summary>
        /// <param name="roleEditDto">Role dto</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Role not found</exception>
        public async Task UpdateAsync(RoleEditDto roleEditDto)
        {
            var roleToUpdate = await _context.Roles.FindAsync(roleEditDto.Id) ??
                               throw new EntityNotFoundException($"{nameof(Role)} - Id: {roleEditDto.Id}");

            roleToUpdate.Description = roleEditDto.Description;

            _context.Entry(roleToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Mark a role as deleted
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Role not found</exception>
        public async Task DeleteAsync(int id)
        {
            var roleToDelete = await _context.Roles.FindAsync(id) ??
                               throw new EntityNotFoundException($"{nameof(Role)} - Id: {id}");

            if (_context.Groups.Any(g => g.GroupRoles.Any(r => r.RoleId == id)) ||
                _context.Users.Any(u => u.UserRoles.Any(r => r.RoleId == id)))
            {
                throw new EntityInUseException(roleToDelete.Description);
            }

            _context.Entry(roleToDelete).State = EntityState.Deleted;
        }

        /// <summary>
        /// Assign an authorization
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="authorizationsId">Authorizations Id</param>
        /// <returns>Task</returns>
        public async Task AssignAuthorizationsAsync(int roleId, IEnumerable<int> authorizationsId)
        {
            var role = await _context.Roles.FindAsync(roleId) ?? throw new EntityNotFoundException(nameof(Role));

            foreach (var id in authorizationsId)
            {
                // Skip already added
                if (role.Authorizations.Any(i => i.DefAuthorizationId == id))
                {
                    continue;
                }

                var authToAdd = await _context.Authorizations.FindAsync(id) ??
                                throw new EntityNotFoundException(nameof(DefinedAuthorization));

                role.AddAuthorization(authToAdd);
            }
        }

        /// <summary>
        /// Remove authorizations
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="authorizationsId">Authorizations id</param>
        /// <returns>Task</returns>
        public async Task RemoveAuthorizationAsync(int roleId, IEnumerable<int> authorizationsId)
        {
            var role = await _context.Roles
                           .Include(i => i.Authorizations)
                           .FirstAsync(i => i.Id == roleId) ??
                       throw new EntityNotFoundException(nameof(Role));

            foreach (var id in authorizationsId)
            {
                var authorizationtoRemove = role.Authorizations.FirstOrDefault(i => i.Id == id);

                if (authorizationtoRemove != null)
                {
                    _context.Entry(authorizationtoRemove).State = EntityState.Deleted;
                }
            }
        }
    }
}