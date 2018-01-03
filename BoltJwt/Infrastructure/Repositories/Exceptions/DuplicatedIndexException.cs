using System;

namespace BoltJwt.Infrastructure.Repositories.Exceptions
{
    public class DuplicatedIndexException : Exception
    {
        public string DuplicatedValue { get; }

        public DuplicatedIndexException(string name)
        {
            DuplicatedValue = name;
        }
    }
}