using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BoltJwt.Application.Commands.Groups;
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

        [Route("")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] GroupInsertCommand groupInsertCommand)
        {
            var result = await _mediator.Send(groupInsertCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("update")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] GroupEditCommand groupEditCommand)
        {
            var result = await _mediator.Send(groupEditCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromQuery] GroupDeleteCommand groupDeleteCommand)
        {
            var result = await _mediator.Send(groupDeleteCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("edit.roles")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditRolesAsync([FromBody] EditGroupRolesCommand editGroupRolesCommand)
        {
            var result = await _mediator.Send(editGroupRolesCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        #endregion
    }
}