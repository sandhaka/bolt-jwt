using System;

namespace BoltJwt.Domain.Exceptions
{
    public class ForbiddenOperationDomainException : Exception
    {
        public ForbiddenOperationDomainException(string description) : base($"Forbidden domain operation: ${description}") { }
    }
}