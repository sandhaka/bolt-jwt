using System;
using System.Threading.Tasks;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Model;
using BoltJwt.Model.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    public class GroupRepository : IRepository<Group>
    {
        private readonly IdentityContext _context;

        public GroupRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a new group to the context
        /// </summary>
        /// <param name="item">Group</param>
        /// <returns>Group added</returns>
        public Group Add(Group item)
        {
            return _context.Groups.Add(item).Entity;
        }

        /// <summary>
        /// Mark the entity as 'modified'.
        /// The entity exists in the database and has been modified on the client.
        /// SaveChanges should send updates for it.
        /// </summary>
        /// <param name="item">Group</param>
        public void Update(Group item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Return the group by id with his properties
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        public async Task<Group> GetAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group != null)
            {
                await _context.Entry(group)
                    .Collection(i => i.GroupRoles).LoadAsync();
                await _context.Entry(group)
                    .Collection(i => i.UserGroups).LoadAsync();
            }

            return group;
        }
    }
}