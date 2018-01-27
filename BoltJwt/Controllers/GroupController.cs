using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BoltJwt.Application.Queries.Groups;
using BoltJwt.Controllers.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class GroupController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGroupQueries _groupQueries;

        public GroupController(IMediator mediator, IGroupQueries groupQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _groupQueries = groupQueries ?? throw new ArgumentNullException(nameof(groupQueries));
        }

        #region [ Queries ]

        [Route("paged")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(PagedData<IEnumerable<object>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PageQuery query)
        {
            var result = await _groupQueries.GetAsync(query);

            return Ok(result);
        }

        [Route("roles")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetRolesAsync([FromUri] int id)
        {
            var result = await _groupQueries.GetRolesAsync(id);

            return Ok(result);
        }

        #endregion

        #region [ Commands ]

        #endregion
    }
}