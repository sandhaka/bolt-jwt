using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class RootUserIsNotEditable : Exception
    {
        public RootUserIsNotEditable() : base("The root user is not editable") { }
    }
}