using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class UnknowAuthorizationException : Exception
    {
        public UnknowAuthorizationException(string authorizationName)
        {
            AuthorizationName = authorizationName;
        }

        public string AuthorizationName { get; }
    }
}