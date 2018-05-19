using System;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model.Aggregates.TokenLog
{
    public class TokenLog : AggregateRoot
    {
        public string Value { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
    }
}