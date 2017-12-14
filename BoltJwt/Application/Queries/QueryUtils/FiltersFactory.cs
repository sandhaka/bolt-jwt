using System;
using System.Collections.Generic;
using System.Linq;
using BoltJwt.Controllers.Filters;
using Newtonsoft.Json;

namespace BoltJwt.Application.Queries.QueryUtils
{
    public static class FiltersFactory
    {
        /// <summary>
        /// Convert to ColumnFilter array
        /// </summary>
        /// <param name="jsonFilters">Json filters object</param>
        /// <returns>Column filters</returns>
        public static ColumnFilter[] ConvertColumnFilters(string jsonFilters)
        {
            if (string.IsNullOrEmpty(jsonFilters))
            {
                return new ColumnFilter[] { };
            }

            try
            {
                var filters = JsonConvert.DeserializeObject<IEnumerable<ColumnFilter>>(jsonFilters);

                return filters as ColumnFilter[] ?? filters.ToArray();
            }
            catch (Exception e)
            {
                throw new QueryFiltersFormatException(jsonFilters, e);
            }
        }
    }
}