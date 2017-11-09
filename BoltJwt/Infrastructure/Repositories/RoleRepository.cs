using System;
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
            var roleToUpdate = await _context.Roles.FindAsync(roleEditDto.Id);

            if (roleToUpdate == null)
            {
                throw new EntityNotFoundException($"{nameof(Role)} - Id: {roleEditDto.Id}");
            }

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
            var roleToDelete = await _context.Roles.FindAsync(id);

            if (roleToDelete == null)
            {
                throw new EntityNotFoundException($"{nameof(Role)} - Id: {id}");
            }

            _context.Entry(roleToDelete).State = EntityState.Deleted;
        }
    }
}