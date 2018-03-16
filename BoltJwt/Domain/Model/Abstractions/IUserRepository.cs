using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Infrastructure.Repositories.Exceptions;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IUserRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Get a user by Id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User entity</returns>
        Task<User> GetAsync(int id);

        /// <summary>
        /// Get user with the all authorizations
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        Task<User> GetWithAutorizationsAsync(int userId);

        /// <summary>
        /// Get user with the all roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        Task<User> GetWithRolesAsync(int userId);

        /// <summary>
        /// Get user with the all groups
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        Task<User> GetWithGroupsAsync(int userId);

        /// <summary>
        /// Get the root user
        /// </summary>
        /// <returns>User</returns>
        Task<User> GetRootAsync();

        /// <summary>
        /// User update
        /// </summary>
        /// <param name="user">User</param>
        void Update(User user);

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User entity</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User added</returns>
        User Add(User user);

        /// <summary>
        /// Update user informations and mark it as modified
        /// </summary>
        /// <param name="userEditDto">User info</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        Task<User> UpdateInfoAsync(UserEditDto userEditDto);

        /// <summary>
        /// Mark a user as deleted
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        Task DeleteAsync(int id);

        /// <summary>
        /// Get user activation code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>User activation code</returns>
        Task<UserActivationCode> GetUserActivationCode(string code);

        /// <summary>
        /// Delete activation code
        /// </summary>
        /// <param name="code">user activation code</param>
        void DeleteUserActivationCode(UserActivationCode code);
    }
}