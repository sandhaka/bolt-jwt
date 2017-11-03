using System.Collections.Generic;
using System.Linq;
using BoltJwt.Model.Abstractions;

namespace BoltJwt.Model
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
        /// <param name="definedAuthorization">Authorization</param>
        public void AddAuthorization(DefinedAuthorization definedAuthorization)
        {
            if (!_authorizations.Any(i => i.AuthorizationName.Equals(definedAuthorization.Name)))
            {
                var authorization = new RoleAuthorization() {AuthorizationName = definedAuthorization.Name};
                _authorizations.Add(authorization);
            }
        }
    }
}