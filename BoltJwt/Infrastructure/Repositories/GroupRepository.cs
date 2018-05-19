using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Domain.Model.Aggregates.Group;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IdentityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get => _context;
        }

        public GroupRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Return group
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        public async Task<Group> GetAsync(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == id) ??
                   throw new EntityNotFoundException(nameof(Group));
        }

        /// <summary>
        /// Return group with his roles collection
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        public async Task<Group> GetWithRolesAsync(int id)
        {
            return await _context.Groups
                    .Include(g => g.GroupRoles)
                    .FirstOrDefaultAsync(g => g.Id == id) ??
                throw new EntityNotFoundException(nameof(Group));
        }

        /// <summary>
        /// Get groups
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Groups</returns>
        public IEnumerable<Group> Qyery(Func<Group, bool> query = null)
        {
            return query == null ?
                _context.Groups :
                _context.Groups.Where(query);
        }

        /// <summary>
        /// Add a new group
        /// </summary>
        /// <param name="group">Group</param>
        /// <returns>Added group</returns>
        public Group Add(Group group)
        {
            _context.Entry(group).State = EntityState.Added;
            return group;
        }

        /// <summary>
        /// Mark a group as deleted
        /// </summary>
        /// <param name="group">Group</param>
        /// <exception cref="EntityInUseException">Group is in use</exception>
        public void Delete(Group group)
        {
            _context.Entry(group).State = EntityState.Deleted;
        }

        /// <summary>
        /// Check group usage
        /// </summary>
        /// <param name="group">Group</param>
        /// <exception cref="EntityInUseException"></exception>
        public void CheckUsage(Group group)
        {
            if (_context.Users.Any(u => u.UserGroups.Any(r => r.GroupId == group.Id)))
            {
                throw new EntityInUseException(group.Description);
            }
        }
    }
}