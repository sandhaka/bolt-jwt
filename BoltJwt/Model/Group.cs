using System.Collections.Generic;
using BoltJwt.Model.Abstractions;

namespace BoltJwt.Model
{
    public class Group : Entity
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

        public Group()
        {
            UserGroups = new List<UserGroup>();
            GroupRoles = new List<GroupRole>();
        }
    }
}