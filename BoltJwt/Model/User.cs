using System.Collections.Generic;
using System.Linq;
using BoltJwt.Model.Abstractions;

namespace BoltJwt.Model
{
    public class User : Entity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Admin is a special user allowed to edit users in BoltJwt except the root
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Root user is the default BoltJwt admin
        /// </summary>
        public bool Root { get; set; }

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