using System;

namespace BoltJwt.Domain.Exceptions
{
    public class AuthorizationCodeDomainException : Exception
    {
        public AuthorizationCodeDomainException(string error) : base(error) { }
    }
}