using System;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.QueryUtils;
using BoltJwt.Controllers.Pagination;
using Dapper;

namespace BoltJwt.Application.Queries.Groups
{
    public class GroupQueries : IGroupQueries
    {
        private readonly string _connectionString;

        public GroupQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Reitreve all groups
        /// </summary>
        /// <returns>Groups</returns>
        public async Task<dynamic> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<dynamic>(
                    @"SELECT Id as id, Description as description FROM IdentityContext.groups ORDER BY Description");
            }
        }

        /// <summary>
        /// Retrieve groups list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged groups data</returns>
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
                    "SELECT COUNT(*) FROM IdentityContext.groups " +
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
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Description ) AS RowNum, * FROM IdentityContext.groups " +
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
                    Data = queryResult.AsList().Select(item => MapGroupObject(item)).ToList(),
                    Page = query
                };
            }
        }

        /// <summary>
        /// Return group roles
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <returns>Roles</returns>
        public async Task<dynamic> GetRolesAsync(int groupId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<dynamic>(
                    @"SELECT Description as role, GroupId as entityId, RoleId as roleId FROM IdentityContext.group_role
                    JOIN IdentityContext.roles ON group_role.RoleId = roles.Id
                    WHERE GroupId = @groupId ORDER BY Description", new { groupId }
                );
            }
        }

        private dynamic MapGroupObject(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.id = result.Id;
            dto.description = result.Description;

            return dto;
        }
    }
}