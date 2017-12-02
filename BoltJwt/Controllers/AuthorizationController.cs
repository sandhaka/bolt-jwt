using System;
using System.Net;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Authorizations;
using BoltJwt.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationQueries _authorizationQueries;

        public AuthorizationController(IAuthorizationQueries authorizationQueries, IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _authorizationQueries =
                authorizationQueries ?? throw new ArgumentNullException(nameof(authorizationQueries));
        }

        [Route("all")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _authorizationQueries.GetAsync();

            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] AuthorizationInsertCommand authorizationInsertCommand)
        {
            var result = await _mediator.Send(authorizationInsertCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(AuthorizationDeleteCommand authorizationDeleteCommand)
        {
            var result = await _mediator.Send(authorizationDeleteCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (ActionResult) BadRequest();
        }
    }
}