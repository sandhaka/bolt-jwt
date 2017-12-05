using System;

namespace BoltJwt.Application.Queries.QueryUtils
{
    public class QueryFiltersFormatException : Exception
    {
        public string Filters { get; }

        public QueryFiltersFormatException(string filters, Exception innerException) :
            base("Filters format exception", innerException)
        {
            Filters = filters;
        }
    }
}