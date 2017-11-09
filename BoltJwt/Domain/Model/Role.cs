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
        /// <param name="authorizationName">Authorization</param>
        public void AddAuthorization(string authorizationName)
        {
            if (!_authorizations.Any(i => i.AuthorizationName.Equals(authorizationName)))
            {
                var authorization = new RoleAuthorization() {AuthorizationName = authorizationName};
                _authorizations.Add(authorization);
            }
        }
    }
}