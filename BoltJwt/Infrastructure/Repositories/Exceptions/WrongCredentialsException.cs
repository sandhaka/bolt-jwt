using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class WrongCredentialsException : Exception
    {
        public WrongCredentialsException() : base("Wrong user name or password")
        {
        }
    }
}