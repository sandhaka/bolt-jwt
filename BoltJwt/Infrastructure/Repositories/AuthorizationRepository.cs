using System;
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
        /// <param name="id">Authorization id</param>
        /// <returns>Task</returns>
        public async Task DeleteAsync(int id)
        {
            var authToDelete = await _context.Authorizations.FindAsync(id);

            if (authToDelete.Name == Constants.AdministrativeAuth)
            {
                throw new NotEditableEntityException($"Authorization definition: {Constants.AdministrativeAuth}");
            }

            _context.Entry(authToDelete).State = EntityState.Deleted;
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
    }
}