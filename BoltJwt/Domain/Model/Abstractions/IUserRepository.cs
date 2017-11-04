using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Infrastructure.Repositories.Exceptions;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IUserRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User added</returns>
        User Add(User user);

        /// <summary>
        /// Update user informations and mark it as modified
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userEditDto">User info</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        Task UpdateAsync(int id, UserEditDto userEditDto);

        /// <summary>
        /// Mark a user as deleted
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        Task DeleteAsync(int id);

        Task UserNameExistsAsync(string username);
        Task EmailExistsAsync(string email);
    }
}