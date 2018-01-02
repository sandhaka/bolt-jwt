using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.QueryUtils;
using BoltJwt.Controllers.Pagination;
using BoltJwt.Domain.Model;
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
        /// Retrieve the authorizations definition list
        /// </summary>
        /// <returns>Authorizations list</returns>
        public async Task<IEnumerable<dynamic>> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    $@"SELECT Id, Name FROM IdentityContext.def_authorizations");

                return result.AsList().Select(r => MapAuthResult(r)).ToList();
            }
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
                    $"SELECT COUNT(*) FROM IdentityContext.def_authorizations " +
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
                    $"SELECT ROW_NUMBER() OVER ( ORDER BY Name ) AS RowNum, * FROM IdentityContext.def_authorizations " +
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

        /// <summary>
        /// Return authorization usage
        /// </summary>
        /// <param name="id">Authorization id</param>
        /// <returns>Lists of user and group names</returns>
        public async Task<dynamic> GetUsageAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var roles = await connection.QueryAsync<dynamic>(
                    @"SELECT Description as roleDescr FROM IdentityContext.roles
                        JOIN IdentityContext.role_authorizations on roles.Id = role_authorizations.RoleId
                    WHERE DefAuthorizationId = @id", new { id }
                );

                var users = await connection.QueryAsync<dynamic>(
                    @"SELECT UserName FROM IdentityContext.users
                        JOIN IdentityContext.user_authorizations on users.Id = user_authorizations.UserId
                    WHERE DefAuthorizationId = @id", new { id }
                );

                return CombineUsageResults(users, roles);
            }
        }

        private dynamic MapAuthResult(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.id = result.Id;
            dto.name = result.Name;

            return dto;
        }

        private dynamic CombineUsageResults(IEnumerable<dynamic> users, IEnumerable<dynamic> roles)
        {
            dynamic result = new ExpandoObject();

            result.roles = roles.Select(i => i.roleDescr);
            result.users = users.Select(i => i.UserName);

            return result;
        }
    }
}