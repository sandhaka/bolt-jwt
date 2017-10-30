using System.Collections.Generic;
using System.Linq;

namespace BoltJwt.Model
{
    public class User : Entity
    {
        public string Name;

        public string Surname;

        public string UserName;

        public string Password;

        public string Email;

        public bool Admin;

        public bool Root;

        /// <summary>
        /// Roles of the user
        /// </summary>
        public List<UserRole> UserRoles { get; }

        /// <summary>
        /// User groups
        /// </summary>
        public List<UserGroup> UserGroups { get; }

        private readonly List<string> _authorizations;

        /// <summary>
        /// Authorization directly assigned
        /// </summary>
        public IReadOnlyCollection<string> Authorizations => _authorizations;

        public User()
        {
            _authorizations = new List<string>();
            UserGroups = new List<UserGroup>();
            UserRoles = new List<UserRole>();
        }

        /// <summary>
        /// Assign an authorization directly
        /// </summary>
        /// <param name="authorization">Authorization</param>
        public void AssignAuthorization(Authorization authorization)
        {
            if (!IsAuthorized(authorization))
            {
                _authorizations.Add(authorization.Name);
            }
        }

        private bool IsAuthorized(Authorization authorization)
        {
            return _authorizations.Any(i => i.Equals(authorization.Name));
        }
    }
}