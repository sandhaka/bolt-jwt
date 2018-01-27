using System;
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
    }
}