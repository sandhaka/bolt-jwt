using System;
using System.Net;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Tokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class TokenServiceController : Controller
    {
        private readonly IMediator _mediator;

        public TokenServiceController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("validate")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Validate([FromBody] ValidateTokenCommand validateTokenCommand)
        {
            var result = await _mediator.Send(validateTokenCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }
    }
}