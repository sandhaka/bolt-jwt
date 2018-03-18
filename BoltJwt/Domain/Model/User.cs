using System;
using System.Collections.Generic;
using System.Linq;
using BoltJwt.Domain.Events;
using BoltJwt.Domain.Exceptions;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Extensions;

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
        /// Update user info
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="username"></param>
        public void Update(string name, string surname, string username)
        {
            if (Root)
            {
                throw new ForbiddenOperationDomainException("Root user");
            }

            Name = name;
            Surname = surname;
            UserName = username;
        }

        /// <summary>
        /// Edit the user password
        /// </summary>
        /// <param name="authorizationCode">Authorization code</param>
        /// <param name="newPassword">Password</param>
        /// <exception cref="AuthorizationCodeDomainException"></exception>
        public void EditPassword(string authorizationCode, string newPassword)
        {
            if (string.IsNullOrEmpty(ForgotPasswordAuthCode))
            {
                throw new AuthorizationCodeDomainException("Illicit request. The user has never requested a password reset");
            }

            if (!ForgotPasswordAuthCode.Equals(authorizationCode))
            {
                throw new AuthorizationCodeDomainException("Wrong authorization code");
            }

            Password = newPassword.ToMd5Hash();
        }

        /// <summary>
        /// Generate a password recovery authorization code
        /// </summary>
        /// <returns>Code</returns>
        public string GenerateForgotPasswordAuthorizationCode()
        {
            ForgotPasswordAuthCode = Guid.NewGuid().ToString();

            return ForgotPasswordAuthCode;
        }

        /// <summary>
        /// Activate user
        /// </summary>
        /// <param name="password">Password</param>
        public void ActivateUser(string password)
        {
            Password = password.ToMd5Hash();
            Disabled = false;
        }

        /// <summary>
        /// Assign / Remove groups
        /// </summary>
        /// <param name="groups">Groups id</param>
        /// <param name="groupEntities">Group entities</param>
        public void EditGroups(IEnumerable<int> groups, List<Group> groupEntities)
        {
            if (Root)
            {
                throw new ForbiddenOperationDomainException("Root user");
            }

            var groupIds = groups as int[] ?? groups.ToArray();

            foreach (var groupId in groupIds)
            {
                var group = groupEntities.FirstOrDefault(g => g.Id == groupId);

                // Skip if the group to add no exists in the context
                if (group == null)
                {
                    continue;
                }

                // Skip if the group is just assigned
                if (UserGroups.Any(i => i.GroupId == groupId))
                {
                    continue;
                }

                UserGroups.Add(
                    new UserGroup
                    {
                        GroupId = group.Id,
                        UserId = Id
                    });
            }

            // Remove deleted groups
            foreach (var userGroup in UserGroups.ToArray())
            {
                if (!groupIds.Contains(userGroup.GroupId))
                {
                    UserGroups.Remove(userGroup);
                }
            }
        }

        /// <summary>
        /// Assign / Remove roles
        /// </summary>
        /// <param name="roles">Roles id</param>
        /// <param name="roleEntities">Role entities</param>
        /// <exception cref="ForbiddenOperationDomainException"></exception>
        public void EditRoles(IEnumerable<int> roles, List<Role> roleEntities)
        {
            if (Root)
            {
                throw new ForbiddenOperationDomainException("Root user");
            }

            var roleIds = roles as int[] ?? roles.ToArray();

            // Add new roles
            foreach (var roleId in roleIds)
            {
                var role = roleEntities.SingleOrDefault(r => r.Id == roleId);

                // Skip if the role to add no exists in the context
                if (role == null)
                {
                    continue;
                }

                // Skip if the role is just assigned
                if (UserRoles.Any(i => i.RoleId == roleId))
                {
                    continue;
                }

                UserRoles.Add(
                    new UserRole
                    {
                        RoleId = role.Id,
                        UserId = Id
                    });
            }

            // Remove deleted roles
            foreach (var userRole in UserRoles.ToArray())
            {
                if (!roleIds.Contains(userRole.RoleId))
                {
                    UserRoles.Remove(userRole);
                }
            }
        }

        /// <summary>
        /// Edit root password
        /// </summary>
        /// <param name="password">Password</param>
        public void EditRootPassword(string password)
        {
            if (Root)
            {
                Password = password.ToMd5Hash();
            }
        }

        /// <summary>
        /// Remove authorizations
        /// </summary>
        /// <param name="authorizationsId">User authorizations id</param>
        /// <returns></returns>
        public void RemoveAuthorizations(IEnumerable<int> authorizationsId)
        {
            if (Root)
            {
                throw new ForbiddenOperationDomainException("Root user");
            }

            Authorizations.RemoveAll(userAuth => authorizationsId.Contains(userAuth.Id));
        }

        /// <summary>
        /// Assign authorizations directly
        /// </summary>
        /// <param name="authorizationsId">Authorizations Id</param>
        /// <returns>Task</returns>
        public void AssignAuthorizations(IEnumerable<int> authorizationsId)
        {
            if (Root)
            {
                throw new ForbiddenOperationDomainException("Root user");
            }

            foreach (var id in authorizationsId)
            {
                // Skip already added authorizations
                if(Authorizations.Any(i => i.DefAuthorizationId == id))
                {
                    continue;
                }

                Authorizations.Add(
                    new UserAuthorization
                    {
                        DefAuthorizationId = id,
                        UserId = Id
                    });
            }
        }
    }
}