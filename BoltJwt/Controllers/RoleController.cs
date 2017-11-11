using System;
using System.Net;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] RoleInsertCommand roleInsertCommand)
        {
            var result = await _mediator.Send(roleInsertCommand);

            return result ? Ok() : (IActionResult) BadRequest();
        }

        [Route("update")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] RoleEditCommand roleEditCommand)
        {
            var result = await _mediator.Send(roleEditCommand);

            return result ? Ok() : (IActionResult) BadRequest();
        }

        [Route("")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromBody] RoleDeleteCommand roleDeleteCommand)
        {
            var result = await _mediator.Send(roleDeleteCommand);

            return result ? Ok() : (IActionResult) BadRequest();
        }
    }
}