using System;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
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
            // TODO: Check if is assigned before!
        }

        /// <summary>
        /// Check if the authorization name has a definition
        /// </summary>
        /// <param name="name">Authorization name</param>
        /// <returns>Authorization is valid</returns>
        public bool ContainsAuthorization(string name)
        {
            return _context.Authorizations.Any(i => i.Name.Equals(name));
        }
    }
}