using System;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.AppConfigurations;
using BoltJwt.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BoltJwt.Infrastructure.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IdentityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get => _context;
        }

        public ConfigurationRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieve configuration data
        /// </summary>
        /// <returns>Configuration</returns>
        public async Task<Configuration> GetAsync()
        {
            return await _context.Configuration.FirstAsync();
        }

        /// <summary>
        /// Update configuration
        /// </summary>
        /// <param name="dto">Dto</param>
        /// <returns>Task</returns>
        public async Task<Configuration> UpdateAsync(ConfigurationDto dto)
        {
            var c = await _context.Configuration.FirstAsync();

            c.SmtpHostName = dto?.SmtpHostName ?? c.SmtpHostName;
            c.SmtpPassword = dto?.SmtpPassword ?? c.SmtpPassword;
            c.SmtpPort = dto?.SmtpPort ?? c.SmtpPort;
            c.SmtpUserName = dto?.SmtpUserName ?? c.SmtpUserName;

            c.EndpointFqdn = dto?.EndpointFqdn ?? c.EndpointFqdn;
            c.EndpointPort = dto?.EndpointPort ?? c.EndpointPort;

            _context.Entry(c).State = EntityState.Modified;

            return c;
        }
    }
}