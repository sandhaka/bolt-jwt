using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.Logs;
using BoltJwt.Controllers.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class TokenLogsController : Controller
    {
        private readonly ITokenLogsQueries _tokenLogsQueries;

        public TokenLogsController(ITokenLogsQueries tokenLogsQueries)
        {
            _tokenLogsQueries = tokenLogsQueries ?? throw new ArgumentNullException(nameof(tokenLogsQueries));
        }

        [Route("")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(PagedData<IEnumerable<object>>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PageQuery query)
        {
            var result = await _tokenLogsQueries.GetAsync(query);

            return Ok(result);
        }
    }
}