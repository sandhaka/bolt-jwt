using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Infrastructure.AppConfigurations
{
    public interface IConfigurationRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Retrieve configuration data
        /// </summary>
        /// <returns>Configuration</returns>
        Task<Configuration> GetAsync();

        /// <summary>
        /// Update configuration
        /// </summary>
        /// <param name="dto">Dto</param>
        /// <returns>Task</returns>
        Task<AppConfigurations.Configuration> UpdateAsync(ConfigurationDto dto);
    }
}