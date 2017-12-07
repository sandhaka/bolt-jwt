using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.QueryUtils;
using BoltJwt.Controllers.Pagination;
using Dapper;

namespace BoltJwt.Application.Queries
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
                    $"WHERE user_authorizations.UserId = {id}");

                return MapUserAuthorizations(result);
            }
        }

        /// <summary>
        /// Retrieve users list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns></returns>
        public async Task<PagedData<dynamic>> GetAsync(PageQuery query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlFilters = SqlCommandsFactory.BuildSqlFilterCommands(query.Filters);

                var count = await connection.QueryAsync<int>(
                    "SELECT COUNT(*) FROM IdentityContext.users WHERE Root = 0" + sqlFilters);

                var startRow = query.PageNumber > 0 ? (query.Size * query.PageNumber) + 1 : 1;
                var endRow = startRow + query.Size;

                var queryResult = await connection.QueryAsync<dynamic>(
                    "SELECT * FROM ( " +
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Email ) AS RowNum, * FROM IdentityContext.users WHERE Root = 0" +
                     sqlFilters +
                    $") AS RowConstrainedResult WHERE RowNum >= ${startRow} AND RowNum < ${endRow} ORDER BY RowNum");

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

        private IEnumerable<dynamic> MapUserAuthorizations(IEnumerable<dynamic> authorizations)
        {
            var dto = new List<dynamic>();

            foreach (var auth in authorizations)
            {
                dynamic item = new ExpandoObject();

                item.entityAuthId = auth.id;
                item.authId = auth.authId;
                item.name = auth.name;

                dto.Add(item);
            }

            return dto;
        }
    }
}