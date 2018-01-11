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
        /// <param name="authorizationsId">Authorizations id</param>
        /// <returns>Task</returns>
        Task AssignAuthorizationAsync(int userId, IEnumerable<int> authorizationsId);

        /// <summary>
        /// Remove authorizations
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="authorizationsId">User authorization id</param>
        /// <returns></returns>
        /// <exception cref="NotEditableEntityException">Root user is not editable</exception>
        Task RemoveAuthorizationAsync(int userId, IEnumerable<int> authorizationsId);

        /// <summary>
        /// Assign a role
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        /// <returns>Task</returns>
        /// <exception cref="NotEditableEntityException">Root user is not editable</exception>
        Task AssignRoleAsync(int userId, int roleId);

        /// <summary>
        /// Activate user, customize password on activation
        /// </summary>
        /// <param name="code">Activation code</param>
        /// <param name="password">New Password</param>
        /// <returns></returns>
        Task ActivateUserAsync(string code, string password);

        /// <summary>
        /// Generate an authorization code to recover the password
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Auth code</returns>
        Task<string> GenerateForgotPasswordAuthorizationCodeAsync(int id);

        /// <summary>
        /// Edit the user password
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="authorizationcode">Authorization code</param>
        /// <param name="newPassword">Password</param>
        /// <returns>Task</returns>
        /// <exception cref="AuthorizationCodeException">Authorization code is missing or wrong</exception>
        Task EditPasswordAsync(int userId, string authorizationcode, string newPassword);

        /// <summary>
        /// Edit the root password
        /// </summary>
        /// <param name="password">Passowrd</param>
        /// <returns>Task</returns>
        Task EditRootPasswordAsync(string password);
    }
}