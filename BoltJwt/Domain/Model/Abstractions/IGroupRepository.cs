using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Aggregates.Group;
using BoltJwt.Infrastructure.Repositories.Exceptions;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IGroupRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Return group
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        Task<Group> GetAsync(int id);

        /// <summary>
        /// Return group with his roles collection
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        Task<Group> GetWithRolesAsync(int id);

        /// <summary>
        /// Get groups
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Groups</returns>
        IEnumerable<Group> Qyery(Func<Group, bool> query = null);

        /// <summary>
        /// Add a new group
        /// </summary>
        /// <param name="group">Group</param>
        /// <returns>Added group</returns>
        Group Add(Group group);

        /// <summary>
        /// Mark a group as deleted
        /// </summary>
        /// <param name="group">Group</param>
        /// <exception cref="EntityInUseException">Group is in use</exception>
        void Delete(Group group);

        /// <summary>
        /// Check group usage
        /// </summary>
        /// <param name="group">Group</param>
        /// <exception cref="EntityInUseException"></exception>
        void CheckUsage(Group group);
    }
}