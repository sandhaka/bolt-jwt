using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Infrastructure.Repositories.Exceptions;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IRoleRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Get role
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        Task<Role> GetAsync(int id);

        /// <summary>
        /// Get role with authorizations
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        Task<Role> GetWithAuthorizationsAsync(int id);

        /// <summary>
        /// Add a new role
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>Role added</returns>
        Role Add(Role role);

        /// <summary>
        /// Get roles
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Roles</returns>
        IEnumerable<Role> Query(Func<Role, bool> query = null);

        /// <summary>
        /// Mark a role as deleted
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Role not found</exception>
        void Delete(Role role);

        /// <summary>
        /// Check role usage
        /// </summary>
        /// <param name="role">Role</param>
        /// <exception cref="EntityInUseException"></exception>
        void CheckUsasge(Role role);
    }
}