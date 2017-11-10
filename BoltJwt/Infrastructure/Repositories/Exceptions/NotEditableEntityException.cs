using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class NotEditableEntityException : Exception
    {
        public NotEditableEntityException(string entityType) : base($"Denied - Not editable, entity: {entityType}") { }
    }
}