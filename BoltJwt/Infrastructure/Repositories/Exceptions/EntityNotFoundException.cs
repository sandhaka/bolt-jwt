using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityTypeName { get; set; }

        public EntityNotFoundException(string entityTypeName)
        {
            EntityTypeName = entityTypeName;
        }
    }
}