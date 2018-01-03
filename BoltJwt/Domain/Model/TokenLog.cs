using System;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class TokenLog : Entity
    {
        public string Value { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
    }
}