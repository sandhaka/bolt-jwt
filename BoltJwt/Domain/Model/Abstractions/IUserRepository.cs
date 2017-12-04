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
        Task<User> UpdateAsync(UserEditDto userEditDto);

        /// <summary>
        /// Mark a user as deleted
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        Task DeleteAsync(int id);

        /// <summary>
        /// Assign an authorization directly
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="authName">Authorization name</param>
        /// <returns>Task</returns>
        Task AssignAuthorizationAsync(int userId, string authName);

        /// <summary>
        /// Assign a role
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        /// <returns>Task</returns>
        Task AssignRoleAsync(int userId, int roleId);

        Task UserNameExistsAsync(string username);
        Task EmailExistsAsync(string email);
    }
}