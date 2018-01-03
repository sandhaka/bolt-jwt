using System.Threading.Tasks;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IAuthorizationRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add a new authorization definition
        /// </summary>
        /// <param name="definedAuthorization">Authorization definition</param>
        /// <returns>Authorization definition</returns>
        DefinedAuthorization Add(DefinedAuthorization definedAuthorization);

        /// <summary>
        /// Delete a authorization if it's not assigned
        /// </summary>
        /// <param name="id">Authorization id</param>
        /// <returns>Task</returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Return a authorization definition by his name
        /// </summary>
        /// <param name="name">Authorization name</param>
        /// <returns>Authorization definition</returns>
        Task<DefinedAuthorization> GetByNameAsync(string name);
    }
}