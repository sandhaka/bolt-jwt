using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Model;
using BoltJwt.Model.Abstractions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BoltJwt.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext _context;

        public UserRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a new user to the context
        /// </summary>
        /// <param name="item">User</param>
        /// <returns>User added</returns>
        public User Add(User item)
        {
            return _context.Users.Add(item).Entity;
        }

        /// <summary>
        /// Mark the entity as 'modified'.
        /// The entity exists in the database and has been modified on the client.
        /// SaveChanges should send updates for it.
        /// </summary>
        /// <param name="item">User</param>
        public void Update(User item)
        {
            // The root user is not editable
            if (item.Root)
            {
                return;
            }

            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Return the user by id with his properties
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        public async Task<User> GetAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(i => i.UserGroups).LoadAsync();
                await _context.Entry(user)
                    .Collection(i => i.UserRoles).LoadAsync();
                await _context.Entry(user)
                    .Collection(i => i.Authorizations).LoadAsync();
            }

            return user;
        }

        /// <summary>
        /// Return the user identity claims
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Identity claims</returns>
        public async Task<ClaimsIdentity> GetIdentityAsync(string username, string password)
        {
            ClaimsIdentity claimsIdentity = null;

            var user = await _context.Users.FirstOrDefaultAsync(i => i.UserName == username);

            if (user != null)
            {
                using (var md5Hash = MD5.Create())
                {
                    var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                    var sBuilder = new StringBuilder();

                    foreach (var t in hash)
                    {
                        sBuilder.Append(t.ToString("x2"));
                    }

                    if (user.Password.Equals(sBuilder.ToString()))
                    {
                        var authorizations = user.Authorizations.Select(i => i.AuthorizationName);

                        claimsIdentity = new ClaimsIdentity(
                            new GenericIdentity(username, "Token"),
                            new []
                            {
                                new Claim("isAdmin", user.Admin ? "true" : "false"),
                                new Claim("isRoot", user.Root ? "true" : "false"),
                                new Claim("userId", user.Id.ToString()),
                                new Claim("username", user.UserName),
                                new Claim("authorizations", JObject.FromObject(authorizations).ToString())
                            });
                    }
                }
            }

            return claimsIdentity;
        }
    }
}