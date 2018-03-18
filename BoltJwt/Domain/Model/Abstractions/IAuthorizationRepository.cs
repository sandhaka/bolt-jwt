using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IAuthorizationRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Return authorization
        /// </summary>
        /// <param name="id">Authorization id</param>
        /// <returns>Authorization</returns>
        Task<DefinedAuthorization> GetAsync(int id);

        /// <summary>
        /// Get authorizations
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Authorizations</returns>
        IEnumerable<DefinedAuthorization> Query(Func<DefinedAuthorization, bool> query = null);

        /// <summary>
        /// Add a new authorization definition
        /// </summary>
        /// <param name="definedAuthorization">Authorization definition</param>
        /// <returns>Authorization definition</returns>
        DefinedAuthorization Add(DefinedAuthorization definedAuthorization);

        /// <summary>
        /// Delete a authorization if it's not assigned
        /// </summary>
        /// <param name="authorization">Authorization</param>
        /// <returns>Task</returns>
        void Delete(DefinedAuthorization authorization);

        /// <summary>
        /// Return a authorization definition by his name
        /// </summary>
        /// <param name="name">Authorization name</param>
        /// <returns>Authorization definition</returns>
        Task<DefinedAuthorization> GetByNameAsync(string name);

        /// <summary>
        /// Check authorization usage
        /// </summary>
        /// <param name="authId">Authorization Id</param>
        /// <returns></returns>
        bool IsInUse(int authId);
    }
}