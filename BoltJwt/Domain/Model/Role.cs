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

        private readonly List<RoleAuthorization> _authorizations;

        /// <summary>
        /// Role authorizations
        /// </summary>
        public IReadOnlyCollection<RoleAuthorization> Authorizations => _authorizations;

        public Role()
        {
            _authorizations = new List<RoleAuthorization>();
            UserRoles = new List<UserRole>();
            GroupRoles = new List<GroupRole>();
        }

        /// <summary>
        /// Add an authorization
        /// </summary>
        /// <param name="authorization">Authorization</param>
        public void AddAuthorization(DefinedAuthorization authorization)
        {
            if (_authorizations.All(i => i.DefAuthorizationId != authorization.Id))
            {
                var roleAuthorization = new RoleAuthorization
                {
                    DefAuthorizationId = authorization.Id,
                    RoleId = Id
                };

                _authorizations.Add(roleAuthorization);
            }
        }
    }
}