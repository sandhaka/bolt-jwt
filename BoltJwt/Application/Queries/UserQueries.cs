using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<dynamic> GetUserAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(@"SELECT * FROM IdentityContext.users WHERE Id = @id and Root = 0", new {id});

                var resultAsArray = result.ToArray();

                if (resultAsArray.Length == 0)
                    throw new KeyNotFoundException();

                return MapUserObject(resultAsArray.First());
            }
        }

        public async Task<IEnumerable<dynamic>> GetUsersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(@"SELECT * FROM IdentityContext.users WHERE Root = 0");

                return result.AsList().Select(item => MapUserObject(item)).ToList();
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