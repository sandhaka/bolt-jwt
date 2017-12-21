using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.QueryUtils;
using BoltJwt.Controllers.Pagination;
using Dapper;

namespace BoltJwt.Application.Queries.Authorizations
{
    /// <summary>
    /// Queries for the 'DefAuthorization' entity
    /// </summary>
    public class AuthorizationQueries : IAuthorizationQueries
    {
        private readonly string _connectionString;

        public AuthorizationQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Retrieve the authorizations definition list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged authorizations data</returns>
        public async Task<PagedData<dynamic>> GetAsync(PageQuery query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Getting filters values
                var sqlFilters = FiltersFactory.ConvertColumnFilters(query.Filters);

                // Configuring filters with wildcards to match substrings also
                var name = sqlFilters.FirstOrDefault(i => i.Name == "name")?.Value;
                var nameValue = string.IsNullOrEmpty(name) ? string.Empty : $"%{name}%";

                var count = await connection.QueryAsync<int>(
                    "SELECT COUNT(*) FROM IdentityContext.def_authorizations WHERE Name <> 'administrative'" +
                    ((!string.IsNullOrEmpty(nameValue)) ? "AND Name like @nameValue " : ""),
                    new
                    {
                        nameValue
                    }
                );

                var startRow = query.PageNumber > 0 ? (query.Size * query.PageNumber) + 1 : 1;
                var endRow = startRow + query.Size;

                var result = await connection.QueryAsync<dynamic>(
                    $@"SELECT * FROM ( " +
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Name ) AS RowNum, * FROM IdentityContext.def_authorizations WHERE Name <> 'administrative' " +
                    ((!string.IsNullOrEmpty(nameValue)) ? "AND name like @nameValue " : "") +
                    ") AS RowConstrainedResult WHERE RowNum >= @startRow AND RowNum < @endRow ORDER BY RowNum",
                    new
                    {
                        nameValue,
                        startRow,
                        endRow
                    }
                );

                // Update pagination data
                query.TotalElements = count.First();
                query.TotalPages = query.TotalElements / query.Size;

                return new PagedData<dynamic>
                {
                    Data = result.AsList().Select(r => MapAuthResult(r)).ToList(),
                    Page = query
                };
            }
        }

        private dynamic MapAuthResult(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.id = result.Id;
            dto.name = result.Name;

            return dto;
        }
    }
}