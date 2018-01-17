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
        /// Add a new role
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>Role added</returns>
        Role Add(Role role);

        /// <summary>
        /// Update role description
        /// </summary>
        /// <param name="roleEditDto">Role dto</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Role not found</exception>
        Task UpdateAsync(RoleEditDto roleEditDto);

        /// <summary>
        /// Mark a role as deleted
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Role not found</exception>
        Task DeleteAsync(int id);

        /// <summary>
        /// Assign an authorization
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="authorizationsId">Authorizations Id</param>
        /// <returns>Task</returns>
        Task AssignAuthorizationsAsync(int roleId, IEnumerable<int> authorizationsId);

        /// <summary>
        /// Remove authorizations
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="authorizationsId">Authorizations id</param>
        /// <returns>Task</returns>
        Task RemoveAuthorizationAsync(int roleId, IEnumerable<int> authorizationsId);
    }
}