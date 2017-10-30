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

        private readonly List<UserAuthorization> _authorizations;

        /// <summary>
        /// Authorization directly assigned
        /// </summary>
        public IReadOnlyCollection<UserAuthorization> Authorizations => _authorizations;

        public User()
        {
            _authorizations = new List<UserAuthorization>();
            UserGroups = new List<UserGroup>();
            UserRoles = new List<UserRole>();
        }

        /// <summary>
        /// Assign an authorization directly
        /// </summary>
        /// <param name="definedAuthorization">Authorization</param>
        public void AssignAuthorization(DefinedAuthorization definedAuthorization)
        {
            if (!IsAuthorized(definedAuthorization))
            {
                var userAuthorization = new UserAuthorization() {AuthorizationName = definedAuthorization.Name};
                _authorizations.Add(userAuthorization);
            }
        }

        private bool IsAuthorized(DefinedAuthorization definedAuthorization)
        {
            return _authorizations.Any(i => i.AuthorizationName.Equals(definedAuthorization.Name));
        }
    }
}