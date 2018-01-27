using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Infrastructure.Repositories.Exceptions;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IGroupRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add a new group
        /// </summary>
        /// <param name="group">Group</param>
        /// <returns>Added group</returns>
        Group Add(Group group);

        /// <summary>
        /// Update group description
        /// </summary>
        /// <param name="groupEditDto">Group dto</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Group not found</exception>
        Task UpdateAsync(GroupEditDto groupEditDto);

        /// <summary>
        /// Mark a group as deleted
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Group not found</exception>
        Task DeleteAsync(int id);
    }
}