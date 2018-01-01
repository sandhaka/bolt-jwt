using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class AuthorizationCodeException : Exception
    {
        public AuthorizationCodeException(string error) : base(error) { }
    }
}