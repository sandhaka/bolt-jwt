using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class EntityInUseException : Exception
    {
        public string Value { get; }

        public EntityInUseException(string val)
        {
            Value = val;
        }
    }
}