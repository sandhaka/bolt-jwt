using System;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class UserActivationCode : Entity
    {
        public int UserId { get; set; }
        public long Timestamp { get; set; }
        public string Code { get; set; }
    }
}