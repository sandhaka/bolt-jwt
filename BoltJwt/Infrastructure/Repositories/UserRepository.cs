using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Domain.Model.Aggregates.User;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ??
                   throw new EntityNotFoundException(nameof(User));
        }

        /// <summary>
        /// Get the root user
        /// </summary>
        /// <returns>User</returns>
        public async Task<User> GetRootAsync()
        {
            return await _context.Users.SingleOrDefaultAsync(i => i.Root) ??
                   throw new EntityNotFoundException(nameof(User));
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
        /// Get user with the all authorizations
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        public async Task<User> GetWithAutorizationsAsync(int userId)
        {
            return await _context.Users
                       .Include(i => i.Authorizations)
                       .FirstOrDefaultAsync(i => i.Id == userId) ??
                   throw new EntityNotFoundException(nameof(User));
        }

        /// <summary>
        /// Get user with the all roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        public async Task<User> GetWithRolesAsync(int userId)
        {
            return await _context.Users
                       .Include(i => i.UserRoles)
                       .FirstOrDefaultAsync(i => i.Id == userId) ??
                   throw new EntityNotFoundException(nameof(User));
        }

        /// <summary>
        /// Get user with the all groups
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        public async Task<User> GetWithGroupsAsync(int userId)
        {
            return await _context.Users
                       .Include(i => i.UserGroups)
                       .FirstOrDefaultAsync(i => i.Id == userId) ??
                   throw new EntityNotFoundException(nameof(User));
        }

        /// <summary>
        /// Get user activation code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>User activation code</returns>
        public async Task<UserActivationCode> GetUserActivationCode(string code)
        {
            return await _context.UserActivationCodes.FirstOrDefaultAsync(i => i.Code == code) ??
                   throw new EntityNotFoundException(nameof(UserActivationCode));
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
        /// User update
        /// </summary>
        /// <param name="user">User</param>
        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        /// <summary>
        /// Check username duplicates
        /// </summary>
        /// <param name="username">Username</param>
        /// <exception cref="DuplicatedIndexException"></exception>
        public void CheckForDuplicates(string username)
        {
            if (_context.Users.Any(i => i.UserName == username))
            {
                throw new DuplicatedIndexException(username);
            }
        }

        /// <summary>
        /// Mark a user as deleted
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Task</returns>
        public void Delete(User user)
        {
            if (user.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            _context.Entry(user).State = EntityState.Deleted;
        }

        /// <summary>
        /// Delete activation code
        /// </summary>
        /// <param name="code">user activation code</param>
        public void DeleteUserActivationCode(UserActivationCode code)
        {
            _context.Entry(code).State = EntityState.Deleted;
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

        /// <summary> TODO: Move from here using dapper
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