using System;
using System.Net;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ActivateAsync([FromBody] UserActivateCommand command)
        {
            var result = await _mediator.Send(command);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("password-recovery")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PasswordRecoveryAsync([FromBody] PasswordRecoveryCommand command)
        {
            var result = await _mediator.Send(command);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("reset-password")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }
    }
}