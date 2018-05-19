using System.Collections.Generic;
using System.Linq;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model.Aggregates.Group
{
    public class Group : AggregateRoot
    {
        public string Description { get; set; }

        /// <summary>
        /// User groups
        /// </summary>
        public List<UserGroup> UserGroups { get; }

        /// <summary>
        /// Roles assigned to the group
        /// </summary>
        public List<GroupRole> GroupRoles { get; }

        public static Group Create(string description)
        {
            return new Group(description);
        }

        private Group(string description)
        {
            Description = description;
            UserGroups = new List<UserGroup>();
            GroupRoles = new List<GroupRole>();
        }

        /// <summary>
        /// Assign / Remove roles
        /// </summary>
        /// <param name="roles">Roles id</param>
        /// <param name="roleEntities">Role entities</param>
        public void EditRoles(IEnumerable<int> roles, List<Role.Role> roleEntities)
        {
            var roleIds = roles as int[] ?? roles.ToArray();

            // Add new roles
            foreach (var roleId in roleIds)
            {
                var role = roleEntities.SingleOrDefault(r => r.Id == roleId);

                if (role == null)
                {
                    continue;
                }

                // Skip if the role is just assigned
                if (GroupRoles.Any(g => g.RoleId == roleId))
                {
                    continue;
                }

                GroupRoles.Add(
                    new GroupRole
                    {
                        RoleId = role.Id,
                        GroupId = Id
                    });
            }

            // Remove deleted roles
            foreach (var groupRole in GroupRoles.ToArray())
            {
                if (!roleIds.Contains(groupRole.RoleId))
                {
                    GroupRoles.Remove(groupRole);
                }
            }
        }
    }
}