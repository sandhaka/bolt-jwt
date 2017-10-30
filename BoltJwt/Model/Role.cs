using System.Collections.Generic;
using System.Linq;

namespace BoltJwt.Model
{
    public class Role : Entity
    {
        public string Description;

        /// <summary>
        /// Users with this role
        /// </summary>
        public List<UserRole> UserRoles { get; }

        /// <summary>
        /// Groups with this role
        /// </summary>
        public List<GroupRole> GroupRoles { get; }

        private readonly List<string> _authorizations;

        /// <summary>
        /// Role authorizations
        /// </summary>
        public IReadOnlyCollection<string> Authorizations => _authorizations;

        public Role()
        {
            _authorizations = new List<string>();
            UserRoles = new List<UserRole>();
            GroupRoles = new List<GroupRole>();
        }

        /// <summary>
        /// Add an authorization
        /// </summary>
        /// <param name="authorization">Authorization</param>
        public void AddAuthorization(Authorization authorization)
        {
            if (!_authorizations.Any(i => i.Equals(authorization.Name)))
            {
                _authorizations.Add(authorization.Name);
            }
        }
    }
}