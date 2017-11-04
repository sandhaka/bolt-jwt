using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    /// <summary>
    /// The property you are trying to add already exists and must be unique
    /// </summary>
    public class PropertyIndexExistsException : Exception
    {
        public string PropertyIndexName { get; set; }

        public PropertyIndexExistsException(string propertyName)
        {
            PropertyIndexName = propertyName;
        }
    }
}