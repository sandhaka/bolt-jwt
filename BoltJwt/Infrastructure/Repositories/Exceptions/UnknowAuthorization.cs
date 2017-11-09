using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class UnknowAuthorization : Exception
    {
        public UnknowAuthorization(string authorizationName)
        {
            AuthorizationName = authorizationName;
        }

        public string AuthorizationName { get; }
    }
}