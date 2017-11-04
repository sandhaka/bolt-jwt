using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class User : Entity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

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

        /// <summary>
        /// Get the comprensive list of the authorizations assigned directly or indirectly though roles or groups
        /// </summary>
        /// <returns>Authorizations list</returns>
        public IEnumerable<string> GetAllAuthorizationsAssigned()
        {
            // Collects in sets to avoid duplicates
            var authorizations = Authorizations.Select(i => i.AuthorizationName).ToHashSet();

            authorizations.UnionWith(
                UserRoles.SelectMany(r =>
                    r.Role.Authorizations.Select(a =>
                        a.AuthorizationName))
                .ToHashSet());

            authorizations.UnionWith(
                UserGroups.SelectMany(g =>
                    g.Group.GroupRoles.SelectMany(gr =>
                        gr.Role.Authorizations.Select(a => a.AuthorizationName)))
                .ToHashSet());

            return authorizations.ToArray();
        }

        public static string PassordEncrypt(string password)
        {
            using (var md5Hash = MD5.Create())
            {
                var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var sBuilder = new StringBuilder();

                foreach (var t in hash)
                {
                    sBuilder.Append(t.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        private bool IsAuthorized(DefinedAuthorization definedAuthorization)
        {
            return _authorizations.Any(i => i.AuthorizationName.Equals(definedAuthorization.Name));
        }
    }
}