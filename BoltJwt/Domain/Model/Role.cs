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
        /// Add authorizations
        /// </summary>
        /// <param name="authorizations">Authorizations id</param>
        /// <param name="authorizationEntities">Authorization entities</param>
        public void AddAuthorizations(IEnumerable<int> authorizations, List<DefinedAuthorization> authorizationEntities)
        {
            foreach (var id in authorizations)
            {
                // Skip already added
                if (Authorizations.Any(i => i.DefAuthorizationId == id))
                {
                    continue;
                }

                var authToAdd = authorizationEntities.SingleOrDefault(a => a.Id == id);

                if (authToAdd != null)
                {
                    var roleAuthorization = new RoleAuthorization
                    {
                        DefAuthorizationId = authToAdd.Id,
                        RoleId = Id
                    };

                    Authorizations.Add(roleAuthorization);
                }
            }
        }

        /// <summary>
        /// Remove authorizations
        /// </summary>
        /// <param name="authorizations">Authorizations id to remove</param>
        public void RemoveAuthorizations(IEnumerable<int> authorizations)
        {
            Authorizations.RemoveAll(a => authorizations.Contains(a.Id));
        }
    }
}