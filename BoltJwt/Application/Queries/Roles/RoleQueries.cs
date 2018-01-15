using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.QueryUtils;
using BoltJwt.Controllers.Pagination;
using Dapper;

namespace BoltJwt.Application.Queries.Roles
{
    /// <summary>
    /// Queries for the 'Role' entity
    /// </summary>
    public class RoleQueries : IRoleQueries
    {
        private readonly string _connectionString;

        public RoleQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Retrieve roles list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged roles data</returns>
        public async Task<PagedData<dynamic>> GetAsync(PageQuery query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Getting filters values
                var sqlFilters = FiltersFactory.ConvertColumnFilters(query.Filters);

                var description = sqlFilters.FirstOrDefault(i => i.Name == "description")?.Value;
                var descriptionValue = string.IsNullOrEmpty(description) ? string.Empty : $"%{description}%";

                var count = await connection.QueryAsync<int>(
                    "SELECT COUNT(*) FROM IdentityContext.roles" +
                    ((!string.IsNullOrEmpty(descriptionValue)) ? "AND Description LIKE @descriptionValue " : ""),
                    new
                    {
                        descriptionValue
                    }
                );

                var startRow = query.PageNumber > 0 ? (query.Size * query.PageNumber) + 1 : 1;
                var endRow = startRow + query.Size;

                var queryResult = await connection.QueryAsync<dynamic>(
                    "SELECT * FROM ( " +
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Description ) AS RowNum, "+
                    "roles.Id as roleId, Description as role, Name as auth FROM IdentityContext.roles " +
                    "JOIN IdentityContext.role_authorizations ON roles.Id = role_authorizations.RoleId " +
                    "JOIN IdentityContext.def_authorizations ON role_authorizations.DefAuthorizationId = def_authorizations.Id " +
                    ((!string.IsNullOrEmpty(descriptionValue)) ? "WHERE Description LIKE @descriptionValue " : "") +
                    ") AS RowConstrainedResult WHERE RowNum >= @startRow AND RowNum < @endRow ORDER BY RowNum",
                    new
                    {
                        descriptionValue,
                        startRow,
                        endRow
                    });

                // Update pagination data
                query.TotalElements = count.First();
                query.TotalPages = query.TotalElements / query.Size;

                return new PagedData<dynamic>
                {
                    Data = queryResult.AsList().Select(item => MapRolesResult(item)).ToList(),
                    Page = query
                };
            }
        }

        private dynamic MapRolesResult(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.Role = result[0].role;
            dto.Id = result[0].roleId;

            dto.Authorizations = new List<dynamic>();

            foreach (var r in result)
            {
                dto.Authorizations.Add(r.authId);
                dto.Authorizations.Add(r.auth);
            }

            return dto;
        }
    }
}