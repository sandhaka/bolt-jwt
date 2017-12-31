using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BoltJwt.Domain.Events;
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

        public bool Disabled { get; set; }

        /// <summary>
        /// Root user is the default BoltJwt admin
        /// </summary>
        public bool Root { get; set; }

        /// <summary>
        /// Forgot password auth code
        /// </summary>
        public string ForgotPasswordAuthCode { get; set; }

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

        /// <summary>
        /// Temporary activation code
        /// </summary>
        public UserActivationCode ActivationCode { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public User()
        {
            Authorizations = new List<UserAuthorization>();
            UserGroups = new List<UserGroup>();
            UserRoles = new List<UserRole>();
        }

        /// <summary>
        /// Parameterized ctor
        /// Init fields and create the domain event. Use this in the business logic
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="activationcode"></param>
        /// <param name="password"></param>
        public User(
            string name, string surname, string username,
            string email, UserActivationCode activationcode,
            string password)
        {
            Name = name;
            Surname = surname;
            UserName = username;
            Email = email;
            ActivationCode = activationcode;
            Password = password;

            this.AddDomainEvent(new UserCreatedDomainEvent
            {
                ActivationCode = ActivationCode.Code,
                Email = Email,
                Name = Name,
                Surname = Surname
            });
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