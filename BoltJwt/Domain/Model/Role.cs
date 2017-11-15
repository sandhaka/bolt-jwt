using System.Collections.Generic;
using System.Linq;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class Role : Entity
    {
        public string Description { get; set; }

        /// <summary>
        /// Users with this role
        /// </summary>
        public List<UserRole> UserRoles { get; }

        /// <summary>
        /// Groups with this role
        /// </summary>
        public List<GroupRole> GroupRoles { get; }

        /// <summary>
        /// Role authorizations
        /// </summary>
        public List<RoleAuthorization> Authorizations { get; set; }

        public Role()
        {
            Authorizations = new List<RoleAuthorization>();
            UserRoles = new List<UserRole>();
            GroupRoles = new List<GroupRole>();
        }

        /// <summary>
        /// Add an authorization
        /// </summary>
        /// <param name="authorization">Authorization</param>
        public void AddAuthorization(DefinedAuthorization authorization)
        {
            if (Authorizations.All(i => i.DefAuthorizationId != authorization.Id))
            {
                var roleAuthorization = new RoleAuthorization
                {
                    DefAuthorizationId = authorization.Id,
                    RoleId = Id
                };

                Authorizations.Add(roleAuthorization);
            }
        }
    }
}