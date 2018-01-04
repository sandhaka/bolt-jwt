using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;

namespace BoltJwt.Infrastructure.Repositories
{
    public class TokenLogsRepository : ITokenLogsRepository
    {
        private readonly IdentityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get => _context;
        }

        public TokenLogsRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a log token
        /// </summary>
        /// <param name="tokenLog">token log</param>
        /// <returns>Task</returns>
        public async Task AddAsync(TokenLog tokenLog)
        {
            await _context.TokenLogs.AddAsync(tokenLog);
        }
    }
}