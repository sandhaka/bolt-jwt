using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.Authorizations;
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
        /// Return a role by id
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        /// <exception cref="KeyNotFoundException">Role not found</exception>
        public async Task<dynamic> GetAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result =
                    await connection.QueryAsync<dynamic>(
                        @"SELECT * FROM IdentityContext.roles WHERE Id = @id", new {id});

                var resultAsArray = result.ToArray();

                if (resultAsArray.Length == 0)
                    throw new KeyNotFoundException();

                return MapRoleObject(resultAsArray.First());
            }
        }

        /// <summary>
        /// Reitreve all roles
        /// </summary>
        /// <returns>Roles</returns>
        public async Task<dynamic> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<dynamic>(
                    @"SELECT Id as id, Description as description FROM IdentityContext.roles ORDER BY Description");
            }
        }

        /// <summary>
        /// Retrieve the role authorizations list
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Authorizations list</returns>
        public async Task<dynamic> GetAuthAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    "SELECT def_authorizations.Id as authId, Name AS name " +
                    "FROM IdentityContext.def_authorizations " +
                    "LEFT JOIN IdentityContext.role_authorizations ON " +
                    "def_authorizations.Id = role_authorizations.DefAuthorizationId " +
                    "WHERE role_authorizations.RoleId = @id ORDER BY Name", new { id });

                return AuthorizationQueries.MapEntityAuthorizations(result);
            }
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
                    "SELECT COUNT(*) FROM IdentityContext.roles " +
                    ((!string.IsNullOrEmpty(descriptionValue)) ? "WHERE Description LIKE @descriptionValue " : ""),
                    new
                    {
                        descriptionValue
                    }
                );

                var startRow = query.PageNumber > 0 ? (query.Size * query.PageNumber) + 1 : 1;
                var endRow = startRow + query.Size;

                var queryResult = await connection.QueryAsync<dynamic>(
                    "SELECT * FROM ( " +
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Description ) AS RowNum, * FROM IdentityContext.roles " +
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
                    Data = queryResult.AsList().Select(item => MapRoleObject(item)).ToList(),
                    Page = query
                };
            }
        }

        /// <summary>
        /// Return role usage
        /// </summary>
        /// <param name="id">role id</param>
        /// <returns>Lists of user and group names</returns>
        public async Task<dynamic> GetUsageAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var groups = await connection.QueryAsync<dynamic>(
                    @"SELECT Description as groupDescr FROM IdentityContext.groups
                        JOIN IdentityContext.group_role on groups.Id = group_role.GroupId
                    WHERE RoleId = @id", new { id }
                );

                var users = await connection.QueryAsync<dynamic>(
                    @"SELECT UserName FROM IdentityContext.users
                        JOIN IdentityContext.user_role on users.Id = user_role.UserId
                    WHERE RoleId = @id", new { id }
                );

                return CombineUsageResults(users, groups);
            }
        }

        private dynamic MapRoleObject(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.id = result.Id;
            dto.description = result.Description;

            return dto;
        }

        private dynamic CombineUsageResults(IEnumerable<dynamic> users, IEnumerable<dynamic> groups)
        {
            dynamic result = new ExpandoObject();

            result.groups = groups.Select(i => i.groupDescr);
            result.users = users.Select(i => i.UserName);

            return result;
        }
    }
}