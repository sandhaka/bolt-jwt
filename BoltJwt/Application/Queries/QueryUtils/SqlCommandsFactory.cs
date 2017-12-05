using System;
using System.Collections.Generic;
using System.Linq;
using BoltJwt.Controllers.Filters;
using Newtonsoft.Json;

namespace BoltJwt.Application.Queries.QueryUtils
{
    public static class SqlCommandsFactory
    {
        /// <summary>
        /// Convert a json filter object to a sql commands
        /// </summary>
        /// <param name="jsonFilters">Json filters object</param>
        /// <returns>Sql commands string</returns>
        public static string BuildSqlFilterCommands(string jsonFilters)
        {
            if (string.IsNullOrEmpty(jsonFilters))
            {
                return string.Empty;
            }

            try
            {
                var filters = JsonConvert.DeserializeObject<IEnumerable<ColumnFilter>>(jsonFilters);

                var columnFilters = filters as ColumnFilter[] ?? filters.ToArray();

                if (!columnFilters.Any())
                {
                    // No filters requested
                    return string.Empty;
                }

                return columnFilters
                    .Select(filter => $" AND {filter.Name} {filter.Operand} '%{filter.Value}%'")
                    .ToList()
                    .Aggregate((a, b) => (a + b));
            }
            catch (Exception e)
            {
                throw new QueryFiltersFormatException(jsonFilters, e);
            }
        }
    }
}