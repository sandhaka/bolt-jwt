﻿using System.Threading.Tasks;
using BoltJwt.Domain.Model.Aggregates.TokenLog;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface ITokenLogsRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add a log token
        /// </summary>
        /// <param name="tokenLog">token log</param>
        /// <returns>Task</returns>
        Task AddAsync(TokenLog tokenLog);
    }
}