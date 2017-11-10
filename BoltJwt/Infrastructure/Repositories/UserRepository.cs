using System;
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
            var userToUpdate = await _context.Users.FindAsync(userEditDto.Id);

            if (userToUpdate == null)
            {
                throw new EntityNotFoundException($"{nameof(User)} - Id: {userEditDto.Id}");
            }

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
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete == null)
            {
                throw new EntityNotFoundException($"{nameof(User)} - Id: {id}");
            }

            if (userToDelete.Root)
            {
                throw new NotEditableEntityException("Root user");
            }

            _context.Entry(userToDelete).State = EntityState.Deleted;
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

            if (user != null)
            {
                if (user.Password.Equals(User.PasswordEncrypt(password)))
                {
                    var authorizations = user.GetAllAuthorizationsAssigned();

                    claimsIdentity = new ClaimsIdentity(
                        new GenericIdentity(username, "Token"),
                        new []
                        {
                            new Claim("isRoot", user.Root ? "true" : "false"),
                            new Claim("userId", user.Id.ToString()),
                            new Claim("username", user.UserName),
                            new Claim("authorizations", JsonConvert.SerializeObject(authorizations))
                        });
                }
            }

            return claimsIdentity;
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