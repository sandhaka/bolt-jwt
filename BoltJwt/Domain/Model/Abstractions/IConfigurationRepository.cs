using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IConfigurationRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Retrieve configuration data
        /// </summary>
        /// <returns>Configuration</returns>
        Task<Configuration> GetAsync();
    }
}