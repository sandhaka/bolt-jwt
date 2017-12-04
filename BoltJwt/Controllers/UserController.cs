using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BoltJwt.Application.Commands.Users;
using BoltJwt.Application.Queries;
using BoltJwt.Controllers.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _userQueries;

        public UserController(IMediator mediator, IUserQueries userQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
        }

        [Route("")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAsync([FromUri] int id)
        {
            var result = await _userQueries.GetAsync(id);

            return Json(new {HttpStatusCode.OK});
        }

        [Route("all")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(PagedData<IEnumerable<object>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PageQuery query)
        {
            var result = await _userQueries.GetAsync(query);

            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] UserInsertCommand userInsertCommand)
        {
            var result = await _mediator.Send(userInsertCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("update")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] UserEditCommand userEditCommand)
        {
            var result = await _mediator.Send(userEditCommand);

            return Json(new {HttpStatusCode.OK, result});
        }

        [Route("")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromBody] UserDeleteCommand userDeleteCommand)
        {
            var result = await _mediator.Send(userDeleteCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("add.auth")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAuthAsync([FromBody] AddAuthorizationUserCommand addAuthorizationUserCommand)
        {
            var result = await _mediator.Send(addAuthorizationUserCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("add.role")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleUserCommand addRoleUserCommand)
        {
            var result = await _mediator.Send(addRoleUserCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }
    }
}