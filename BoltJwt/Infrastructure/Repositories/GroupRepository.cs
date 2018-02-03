using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
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
        /// Update group description
        /// </summary>
        /// <param name="groupEditDto">Group dto</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Group not found</exception>
        public async Task UpdateAsync(GroupEditDto groupEditDto)
        {
            var groupToUpdate = await _context.Groups.FindAsync(groupEditDto.Id) ??
                               throw new EntityNotFoundException($"{nameof(Group)} - Id: {groupEditDto.Id}");

            groupToUpdate.Description = groupEditDto.Description;

            _context.Entry(groupToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Mark a group as deleted
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">Group not found</exception>
        public async Task DeleteAsync(int id)
        {
            var groupToDelete = await _context.Groups.FindAsync(id) ??
                               throw new EntityNotFoundException($"{nameof(Group)} - Id: {id}");

            if (_context.Users.Any(u => u.UserGroups.Any(r => r.GroupId == id)))
            {
                throw new EntityInUseException(groupToDelete.Description);
            }

            _context.Entry(groupToDelete).State = EntityState.Deleted;
        }

        /// <summary>
        /// Assign / Remove roles
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <param name="roles">Roles id</param>
        /// <returns>Task</returns>
        public async Task EditRolesAsync(int groupId, IEnumerable<int> roles)
        {
            var group = await _context.Groups
                            .Include(g => g.GroupRoles)
                            .FirstAsync(g => g.Id == groupId) ??
                        throw new EntityNotFoundException(nameof(Group));

            var roleIds = roles as int[] ?? roles.ToArray();

            // Add new roles
            foreach (var roleId in roleIds)
            {
                var role = _context.Roles.FindAsync(roleId) ?? throw new EntityNotFoundException(nameof(Role));

                // Skip if the role is just assigned
                if (group.GroupRoles.Any(g => g.RoleId == roleId))
                {
                    continue;
                }

                group.GroupRoles.Add(
                    new GroupRole
                    {
                        RoleId = role.Id,
                        GroupId = group.Id
                    });
            }

            // Remove deleted roles
            foreach (var groupRole in group.GroupRoles.ToArray())
            {
                if (!roleIds.Contains(groupRole.RoleId))
                {
                    group.GroupRoles.Remove(groupRole);
                }
            }
        }
    }
}