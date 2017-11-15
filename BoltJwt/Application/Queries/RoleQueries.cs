using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace BoltJwt.Application.Queries
{
    public class RoleQueries : IRoleQueries
    {
        private readonly string _connectionString;

        public RoleQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<dynamic> GetRolesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"SELECT r.Description as role, da.Name as auth FROM IdentityContext.roles r
                        JOIN IdentityContext.role_authorizations ra ON r.Id = ra.RoleId
                        JOIN IdentityContext.def_authorizations da ON ra.DefAuthorizationId = da.Id");

                return MapRolesResult(result);
            }
        }

        private dynamic MapRolesResult(dynamic result)
        {
            dynamic dto = new ExpandoObject();

            dto.Role = result[0].role;

            dto.Authorizations = new List<dynamic>();

            foreach (var r in result)
            {
                dto.Authorizations.Add(r.auth);
            }

            return dto;
        }
    }
}