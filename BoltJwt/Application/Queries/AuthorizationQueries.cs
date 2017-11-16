using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using Dapper;

namespace BoltJwt.Application.Queries
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

        public async Task<IEnumerable<dynamic>> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    $@"SELECT Id as id, Name as auth FROM IdentityContext.def_authorizations
                        WHERE Name <> '{Constants.AdministrativeAuth}'");

                return result.AsList().Select(r => MapAuthResult(r)).ToList();
            }
        }

        private dynamic MapAuthResult(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.id = result.id;
            dto.authorization = result.auth;

            return dto;
        }
    }
}