using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Infrastructure.Repositories.Exceptions;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IGroupRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Get groups
        /// </summary>
        /// <param name="groupsId">Groups id</param>
        /// <returns>Groups</returns>
        IEnumerable<Group> GetAll();

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

        /// <summary>
        /// Assign / Remove roles
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <param name="roles">Roles id</param>
        /// <returns>Task</returns>
        Task EditRolesAsync(int groupId, IEnumerable<int> roles);
    }
}