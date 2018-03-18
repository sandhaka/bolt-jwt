using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    /// <summary>
    /// Authorization definitions
    /// </summary>
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly IdentityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get => _context;
        }

        public AuthorizationRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Return authorization
        /// </summary>
        /// <param name="id">Authorization id</param>
        /// <returns>Authorization</returns>
        public async Task<DefinedAuthorization> GetAsync(int id)
        {
            return await _context.Authorizations.FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Get authorizations
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Authorizations</returns>
        public IEnumerable<DefinedAuthorization> Query(Func<DefinedAuthorization, bool> query = null)
        {
            return query == null ?
                _context.Authorizations :
                _context.Authorizations.Where(query);
        }

        /// <summary>
        /// Add a new authorization definition
        /// </summary>
        /// <param name="definedAuthorization">Authorization definition</param>
        /// <returns>Authorization definition</returns>
        public DefinedAuthorization Add(DefinedAuthorization definedAuthorization)
        {
            if (_context.Authorizations.Any(i => i.Name == definedAuthorization.Name))
            {
                throw new DuplicatedIndexException(definedAuthorization.Name);
            }

            _context.Add(definedAuthorization).State = EntityState.Added;
            return definedAuthorization;
        }

        /// <summary>
        /// Delete a authorization if it's not assigned
        /// </summary>
        /// <param name="authorization">Authorization</param>
        /// <returns>Task</returns>
        public void Delete(DefinedAuthorization authorization)
        {
            if (authorization.Name == Constants.AdministrativeAuth)
            {
                throw new NotEditableEntityException($"Authorization definition: {Constants.AdministrativeAuth}");
            }

            if (IsInUse(authorization.Id))
            {
                throw new EntityInUseException(authorization.Name);
            }

            _context.Entry(authorization).State = EntityState.Deleted;
        }

        /// <summary>
        /// Return a authorization definition by his name
        /// </summary>
        /// <param name="name">Authorization name</param>
        /// <returns>Authorization definition</returns>
        public async Task<DefinedAuthorization> GetByNameAsync(string name)
        {
            return await _context.Authorizations.FirstAsync(i => i.Name == name);
        }

        /// <summary>
        /// Check authorization usage
        /// </summary>
        /// <param name="authId">Authorization Id</param>
        /// <returns></returns>
        public bool IsInUse(int authId)
        {
            return _context.Roles.Any(i => i.Authorizations.Any(j => j.DefAuthorizationId == authId)) ||
                   _context.Users.Any(i => i.Authorizations.Any(j => j.DefAuthorizationId == authId));
        }
    }
}