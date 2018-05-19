using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Domain.Model.Aggregates.Configuration;
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
    }
}