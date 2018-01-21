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

namespace BoltJwt.Application.Queries.Users
{
    /// <summary>
    /// Queries for the 'User' entity
    /// </summary>
    public class UserQueries : IUserQueries
    {
        private readonly string _connectionString;

        public UserQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Return a user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User info</returns>
        /// <exception cref="KeyNotFoundException">User not found</exception>
        public async Task<dynamic> GetAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result =
                    await connection.QueryAsync<dynamic>(
                        @"SELECT * FROM IdentityContext.users WHERE Id = @id and Root = 0", new {id});

                var resultAsArray = result.ToArray();

                if (resultAsArray.Length == 0)
                    throw new KeyNotFoundException();

                return MapUserObject(resultAsArray.First());
            }
        }

        /// <summary>
        /// Retrieve the user authorizations list
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Authorizations list</returns>
        public async Task<dynamic> GetAuthAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    "SELECT def_authorizations.Id as authId, user_authorizations.Id AS id, Name AS name " +
                    "FROM IdentityContext.def_authorizations " +
                    "LEFT JOIN IdentityContext.user_authorizations ON " +
                        "def_authorizations.Id = user_authorizations.DefAuthorizationId " +
                    @"WHERE user_authorizations.UserId = @id", new { id });

                return AuthorizationQueries.MapEntityAuthorizations(result);
            }
        }

        /// <summary>
        /// Retrieve users list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged users data</returns>
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

                var surname = sqlFilters.FirstOrDefault(i => i.Name == "surname")?.Value;
                var surnameValue = string.IsNullOrEmpty(surname) ? string.Empty : $"%{surname}%";

                var username = sqlFilters.FirstOrDefault(i => i.Name == "username")?.Value;
                var usernameValue = string.IsNullOrEmpty(username) ? string.Empty : $"%{username}%";

                var email = sqlFilters.FirstOrDefault(i => i.Name == "email")?.Value;
                var emailValue = string.IsNullOrEmpty(email) ? string.Empty : $"%{email}%";

                // Get the total rows count
                var count = await connection.QueryAsync<int>(
                    "SELECT COUNT(*) FROM IdentityContext.users WHERE Root = 0 AND Disabled = 0 " +
                    ((!string.IsNullOrEmpty(nameValue)) ? "AND name LIKE @nameValue " : "") +
                    ((!string.IsNullOrEmpty(surnameValue)) ? "AND surname LIKE @surnameValue " : "") +
                    ((!string.IsNullOrEmpty(usernameValue)) ? "AND username LIKE @usernameValue " : "") +
                    ((!string.IsNullOrEmpty(emailValue)) ? "AND email LIKE @emailValue " : ""),
                    new
                    {
                        nameValue,
                        surnameValue,
                        usernameValue,
                        emailValue,
                    });

                var startRow = query.PageNumber > 0 ? (query.Size * query.PageNumber) + 1 : 1;
                var endRow = startRow + query.Size;

                // Get the paged users data
                var queryResult = await connection.QueryAsync<dynamic>(
                    "SELECT * FROM ( " +
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Email ) AS RowNum, * FROM IdentityContext.users WHERE Root = 0 AND Disabled = 0 " +
                    ((!string.IsNullOrEmpty(nameValue)) ? "AND name LIKE @nameValue " : "") +
                    ((!string.IsNullOrEmpty(surnameValue)) ? "AND surname LIKE @surnameValue " : "") +
                    ((!string.IsNullOrEmpty(usernameValue)) ? "AND username LIKE @usernameValue " : "") +
                    ((!string.IsNullOrEmpty(emailValue)) ? "AND email LIKE @emailValue " : "") +
                    ") AS RowConstrainedResult WHERE RowNum >= @startRow AND RowNum < @endRow ORDER BY RowNum",
                    new
                    {
                        nameValue,
                        surnameValue,
                        usernameValue,
                        emailValue,
                        startRow,
                        endRow
                    });

                // Update pagination data
                query.TotalElements = count.First();
                query.TotalPages = query.TotalElements / query.Size;

                return new PagedData<dynamic>
                {
                    Data = queryResult.AsList().Select(item => MapUserObject(item)).ToList(),
                    Page = query
                };
            }
        }

        /// <summary>
        /// Return user roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Roles</returns>
        public async Task<dynamic> GetRolesAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<dynamic>(
                    @"SELECT Description as role, UserId as assignedRoleId, RoleId as roleId FROM IdentityContext.user_role
                    JOIN IdentityContext.roles ON user_role.RoleId = roles.Id
                    WHERE UserId = @userId ORDER BY Description", new { userId }
                    );
            }
        }

        private dynamic MapUserObject(dynamic user)
        {
            dynamic dto = new ExpandoObject();

            dto.id = user.Id;
            dto.email = user.Email;
            dto.name = user.Name;
            dto.surname = user.Surname;
            dto.username = user.UserName;

            return dto;
        }
    }
}