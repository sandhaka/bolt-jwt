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

        /// <summary>
        /// Authorization directly assigned
        /// </summary>
        public List<UserAuthorization> Authorizations { get; set; }

        public User()
        {
            Authorizations = new List<UserAuthorization>();
            UserGroups = new List<UserGroup>();
            UserRoles = new List<UserRole>();
        }

        /// <summary>
        /// Check if the authorization is assigned
        /// </summary>
        /// <param name="definedAuthorization">Authorization</param>
        /// <returns>Assigned</returns>
        public bool IsAuthorized(DefinedAuthorization definedAuthorization)
        {
            return Authorizations.Any(i => i.DefAuthorizationId == definedAuthorization.Id);
        }

        public static string PasswordEncrypt(string password)
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
    }
}