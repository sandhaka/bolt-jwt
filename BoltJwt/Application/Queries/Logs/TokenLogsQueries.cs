using System;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Controllers.Pagination;
using Dapper;

namespace BoltJwt.Application.Queries.Logs
{
    public class TokenLogsQueries : ITokenLogsQueries
    {
        private readonly string _connectionString;

        public TokenLogsQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Retrieve generated token list with pagination
        /// </summary>
        /// <param name="query">Query page parameters</param>
        /// <returns>Paged data</returns>
        public async Task<PagedData<dynamic>> GetAsync(PageQuery query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var count = await connection.QueryAsync<int>("SELECT COUNT(*) FROM IdentityContext.tokenlogs");

                var startRow = query.PageNumber > 0 ? (query.Size * query.PageNumber) + 1 : 1;
                var endRow = startRow + query.Size;

                var queryResult = await connection.QueryAsync<dynamic>(
                    "SELECT * FROM ( " +
                    "SELECT ROW_NUMBER() OVER ( ORDER BY Timestamp ) AS RowNum, * FROM IdentityContext.tokenlogs " +
                    ") AS RowConstrainedResult WHERE RowNum >= @startRow AND RowNum < @endRow ORDER BY RowNum",
                    new
                    {
                        startRow,
                        endRow
                    }
                );

                // Update pagination data
                query.TotalElements = count.First();
                query.TotalPages = query.TotalElements / query.Size;

                return new PagedData<dynamic>
                {
                    Data = queryResult.AsList().Select(item => MapTokenLogObject(item)).ToList(),
                    Page = query
                };
            }
        }

        private dynamic MapTokenLogObject(dynamic tokenLog)
        {
            dynamic dto = new ExpandoObject();

            dto.userId = tokenLog.UserId;
            dto.value = tokenLog.Value;
            dto.timestamp = tokenLog.Timestamp;

            return dto;
        }
    }
}