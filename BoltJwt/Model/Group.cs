using System.Collections.Generic;

namespace BoltJwt.Model
{
    public class Group : Entity
    {
        public string Description;

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