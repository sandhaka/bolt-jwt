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
        /// Add a new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User added</returns>
        public User Add(User user)
        {
            _context.Entry(user).State = EntityState.Added;
            return user;
        }

        /// <summary>
        /// Update user informations
        /// </summary>
        /// <param name="userEditDto">User info</param>
        /// <returns>Task</returns>
        /// <exception cref="EntityNotFoundException">User not found</exception>
        public async Task UpdateAsync(UserEditDto userEditDto)
        {
            var userToUpdate = await _context.Users.FindAsync(userEditDto.Id) ??
                               throw new EntityNotFoundException($"{nameof(User)} - Id: {userEditDto.Id}");

            if (userToUpdate.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            userToUpdate.Name = userEditDto.Name;
            userToUpdate.Surname = userEditDto.Surname;
            userToUpdate.UserName = userEditDto.UserName;

            _context.Entry(userToUpdate).State = EntityState.Modified;
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
        /// <param name="authName">Authorization name</param>
        /// <returns>Task</returns>
        public async Task AssignAuthorizationAsync(int userId, string authName)
        {
            var authorization = await _context.Authorizations.FirstAsync(i => i.Name == authName) ??
                                throw new EntityNotFoundException(nameof(DefinedAuthorization));

            var user = await _context.Users.FindAsync(userId) ?? throw new EntityNotFoundException(nameof(User));

            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            user.Authorizations.Add(
                new UserAuthorization
                {
                    DefAuthorizationId = authorization.Id,
                    UserId = user.Id
                });
        }

        /// <summary>
        /// Assign a role
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        /// <returns>Task</returns>
        public async Task AssignRoleAsync(int userId, int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId) ??
                       throw new EntityNotFoundException(nameof(Role));

            var user = await _context.Users.FindAsync(userId) ?? throw new EntityNotFoundException(nameof(User));

            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            user.UserRoles.Add(
                new UserRole
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
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

            if (user == null || !user.Password.Equals(User.PasswordEncrypt(password)))
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

        #region [Helpers]

        public async Task UserNameExistsAsync(string username)
        {
            if (await _context.Users.AnyAsync(i => i.UserName.Equals(username)))
            {
                throw new PropertyIndexExistsException("UserName");
            }
        }

        public async Task EmailExistsAsync(string email)
        {
            if (await _context.Users.AnyAsync(i => i.Email.Equals(email)))
            {
                throw new PropertyIndexExistsException("Email");
            }
        }

        #endregion
    }
}