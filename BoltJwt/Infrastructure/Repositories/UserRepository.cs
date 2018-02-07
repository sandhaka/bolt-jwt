using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Infrastructure.Extensions;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BoltJwt.Infrastructure.Repositories
{
    /// <summary>
    /// Define user operations
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get => _context;
        }

        public UserRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get a user by Id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User entity</returns>
        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User entity</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Email == email) ??
                   throw new EntityNotFoundException(nameof(User));
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User added</returns>
        public User Add(User user)
        {
            if (_context.Users.Any(i => i.UserName == user.UserName))
            {
                throw new DuplicatedIndexException(user.UserName);
            }

            if (_context.Users.Any(i => i.Email == user.Email))
            {
                throw new DuplicatedIndexException(user.Email);
            }

            _context.Users.Add(user);

            return user;
        }

        /// <summary>
        /// Update user informations
        /// </summary>
        /// <param name="userEditDto">User info</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        public async Task<User> UpdateAsync(UserEditDto userEditDto)
        {
            var userToUpdate = await _context.Users.FindAsync(userEditDto.Id) ??
                               throw new EntityNotFoundException($"{nameof(User)} - Id: {userEditDto.Id}");

            if (userToUpdate.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            if (_context.Users.Any(i => i.UserName == userEditDto.UserName))
            {
                throw new DuplicatedIndexException(userEditDto.UserName);
            }

            userToUpdate.Name = userEditDto.Name;
            userToUpdate.Surname = userEditDto.Surname;
            userToUpdate.UserName = userEditDto.UserName;

            var entry = _context.Entry(userToUpdate);

            entry.State = EntityState.Modified;

            return entry.Entity;
        }

        /// <summary>
        /// Mark a user as deleted
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        public async Task DeleteAsync(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id) ??
                               throw new EntityNotFoundException($"{nameof(User)} - Id: {id}");

            if (userToDelete.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            _context.Entry(userToDelete).State = EntityState.Deleted;
        }

        /// <summary>
        /// Assign an authorization directly
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="authorizationsId">Authorizations Id</param>
        /// <returns>Task</returns>
        /// <exception cref="NotEditableEntityException">Root user is not editable</exception>
        public async Task AssignAuthorizationAsync(int userId, IEnumerable<int> authorizationsId)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new EntityNotFoundException(nameof(User));

            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            foreach (var id in authorizationsId)
            {
                // Skip already added authorizations
                if(user.Authorizations.Any(i => i.DefAuthorizationId == id))
                {
                    continue;
                }

                user.Authorizations.Add(
                    new UserAuthorization
                    {
                        DefAuthorizationId = id,
                        UserId = user.Id
                    });
            }
        }

        /// <summary>
        /// Remove authorizations
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="authorizationsId">User authorization id</param>
        /// <returns></returns>
        /// <exception cref="NotEditableEntityException">Root user is not editable</exception>
        public async Task RemoveAuthorizationAsync(int userId, IEnumerable<int> authorizationsId)
        {
            var user = await _context.Users
                           .Include(i => i.Authorizations)
                           .FirstAsync(i => i.Id == userId) ??
                       throw new EntityNotFoundException(nameof(User));

            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            foreach (var id in authorizationsId)
            {
                var authorizationtoRemove = user.Authorizations.FirstOrDefault(i => i.Id == id);

                if (authorizationtoRemove != null)
                {
                    _context.Entry(authorizationtoRemove).State = EntityState.Deleted;
                }
            }
        }

        /// <summary>
        /// Assign / Remove roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roles">Roles id</param>
        /// <returns>Task</returns>
        /// <exception cref="NotEditableEntityException">Root user is not editable</exception>
        public async Task EditRolesAsync(int userId, IEnumerable<int> roles)
        {
            var user = await _context.Users
                           .Include(u => u.UserRoles)
                           .FirstAsync(u => u.Id == userId) ??
                       throw new EntityNotFoundException(nameof(User));

            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            var roleIds = roles as int[] ?? roles.ToArray();

            // Add new roles
            foreach (var roleId in roleIds)
            {
                var role = await _context.Roles.FindAsync(roleId) ??
                           throw new EntityNotFoundException(nameof(Role));

                // Skip if the role is just assigned
                if (user.UserRoles.Any(i => i.RoleId == roleId))
                {
                    continue;
                }

                user.UserRoles.Add(
                    new UserRole
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });
            }

            // Remove deleted roles
            foreach (var userRole in user.UserRoles.ToArray())
            {
                if (!roleIds.Contains(userRole.RoleId))
                {
                    user.UserRoles.Remove(userRole);
                }
            }
        }

        /// <summary>
        /// Assign / Remove groups
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="groups">Groups id</param>
        /// <returns>Task</returns>
        /// <exception cref="NotEditableEntityException">Root user is not editable</exception>
        public async Task EditGroupsAsync(int userId, IEnumerable<int> groups)
        {
            var user = await _context.Users
                           .Include(u => u.UserGroups)
                           .FirstAsync(u => u.Id == userId) ??
                       throw new EntityNotFoundException(nameof(User));

            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            var groupIds = groups as int[] ?? groups.ToArray();

            // Add new group
            foreach (var groupId in groupIds)
            {
                var group = await _context.Groups.FindAsync(groupId) ??
                           throw new EntityNotFoundException(nameof(Group));

                // Skip if the group is just assigned
                if (user.UserGroups.Any(i => i.GroupId == groupId))
                {
                    continue;
                }

                user.UserGroups.Add(
                    new UserGroup
                    {
                        GroupId = group.Id,
                        UserId = user.Id
                    });
            }

            // Remove deleted groups
            foreach (var userGroup in user.UserGroups.ToArray())
            {
                if (!groupIds.Contains(userGroup.GroupId))
                {
                    user.UserGroups.Remove(userGroup);
                }
            }
        }

        /// <summary>
        /// Activate user, customize password on activation
        /// </summary>
        /// <param name="code">Activation code</param>
        /// <param name="password">New Password</param>
        /// <returns></returns>
        public async Task ActivateUserAsync(string code, string password)
        {
            var userActivationCode = await _context.UserActivationCodes.FirstAsync(i => i.Code == code) ??
                                     throw new EntityNotFoundException(nameof(UserActivationCode));

            var user = await _context.Users.FindAsync(userActivationCode.UserId) ??
                       throw new EntityNotFoundException(nameof(User));

            // If the user exists and if it's wainting for activation, edit his password and activate
            user.Password = password.ToMd5Hash();

            user.Disabled = false;

            _context.Entry(user).State = EntityState.Modified;

            // Delete the activation code
            _context.Entry(userActivationCode).State = EntityState.Deleted;
        }

        /// <summary>
        /// Generate an authorization code to recover the password
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Auth code</returns>
        public async Task<string> GenerateForgotPasswordAuthorizationCodeAsync(int id)
        {
            var user = await GetAsync(id);

            user.ForgotPasswordAuthCode = Guid.NewGuid().ToString();

            _context.Entry(user).State = EntityState.Modified;

            return user.ForgotPasswordAuthCode;
        }

        /// <summary>
        /// Edit the user password
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="authorizationcode">Authorization code</param>
        /// <param name="newPassword">Password</param>
        /// <returns>Task</returns>
        /// <exception cref="AuthorizationCodeException">Authorization code is missing or wrong</exception>
        public async Task EditPasswordAsync(int userId, string authorizationcode, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new EntityNotFoundException(nameof(User));

            if (string.IsNullOrEmpty(user.ForgotPasswordAuthCode))
            {
                throw new AuthorizationCodeException("Illicit request. The user has never requested a password reset");
            }

            if (!user.ForgotPasswordAuthCode.Equals(authorizationcode))
            {
                throw new AuthorizationCodeException("Wrong authorization code");
            }

            user.Password = newPassword.ToMd5Hash();

            _context.Entry(user).State = EntityState.Modified;
        }

        /// <summary>
        /// Edit the root password
        /// </summary>
        /// <param name="password">Passowrd</param>
        /// <returns>Task</returns>
        public async Task EditRootPasswordAsync(string password)
        {
            var user = await _context.Users.FirstAsync(i => i.Root);

            user.Password = password.ToMd5Hash();
        }

        /// <summary>
        /// Return the user identity claims
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Identity claims</returns>
        public static async Task<ClaimsIdentity> GetIdentityAsync(IdentityContext context, string username, string password)
        {
            ClaimsIdentity claimsIdentity = null;

             var user = await context.Users
                .Include(i=>i.Authorizations)
                .Include(i=>i.UserGroups)
                .Include(i=>i.UserRoles)
                .FirstOrDefaultAsync(i => i.UserName == username);

            if (user == null || !user.Password.Equals(password.ToMd5Hash()))
            {
                return null;
            }

            var authorizations = GetAllAuthorizationsAssigned(context, user);

            claimsIdentity = new ClaimsIdentity(
                new GenericIdentity(username, "Token"),
                new []
                {
                    new Claim("isRoot", user.Root ? "true" : "false"),
                    new Claim("userId", user.Id.ToString()),
                    new Claim("username", user.UserName),
                    new Claim("authorizations", JsonConvert.SerializeObject(authorizations))
                });

            return claimsIdentity;
        }

        /// <summary>
        /// Get the comprensive list of the authorizations assigned directly or indirectly trough roles or groups
        /// </summary>
        /// <returns>Authorization names list</returns>
        private static IEnumerable<string> GetAllAuthorizationsAssigned(IdentityContext context, User user)
        {
            // Authorizations directly assigned:
            var authorizations = from auths in context.Authorizations
                join userAuth in user.Authorizations on auths.Id equals userAuth.DefAuthorizationId
                select auths;

            // Authorization assigned through user roles
            var authorizationsFromRoles = from auths in context.Authorizations
                join roleAuth in (
                    from uRoles in context.Roles
                    join userRole in user.UserRoles on uRoles.Id equals userRole.RoleId
                    select uRoles).SelectMany(i => i.Authorizations).ToArray() on auths.Id equals roleAuth.DefAuthorizationId
                select auths;

            // Authorization assigned through roles of the user groups
            var authorizationsFromRolesGroups = from auths in context.Authorizations
                join roleAuth in (
                    from uRoles in context.Roles
                    join groupRole in (
                        from groups in context.Groups
                        join userGroup in user.UserGroups on groups.Id equals userGroup.GroupId
                        select groups).SelectMany(i => i.GroupRoles).ToArray() on uRoles.Id equals groupRole.RoleId
                    select uRoles
                ).SelectMany(i => i.Authorizations).ToArray() on auths.Id equals roleAuth.DefAuthorizationId
                select auths;

            // Add all in a HashSet to avoid duplicates
            var authorizationSet = authorizations.ToHashSet();

            authorizationSet.UnionWith(authorizationsFromRoles);
            authorizationSet.UnionWith(authorizationsFromRolesGroups);

            return authorizationSet.Select(i => i.Name).ToArray();
        }
    }
}