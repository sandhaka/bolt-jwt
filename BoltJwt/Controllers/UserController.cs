﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BoltJwt.Application.Commands.Users;
using BoltJwt.Application.Queries.Users;
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

        #region [ Queries ]

        [Route("")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAsync([FromUri] int id)
        {
            var result = await _userQueries.GetAsync(id);

            return Json(new {HttpStatusCode.OK});
        }

        [Route("paged")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(PagedData<IEnumerable<object>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PageQuery query)
        {
            var result = await _userQueries.GetAsync(query);

            return Ok(result);
        }

        [Route("authorizations")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAuthAsync([FromUri] int id)
        {
            var result = await _userQueries.GetAuthAsync(id);

            return Ok(result);
        }

        [Route("roles")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetRolesAsync([FromUri] int id)
        {
            var result = await _userQueries.GetRolesAsync(id);

            return Ok(result);
        }

        [Route("groups")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetGroupsAsync([FromUri] int id)
        {
            var result = await _userQueries.GetGroupsAsync(id);

            return Ok(result);
        }

        #endregion

        #region [ Commands ]

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
        public async Task<IActionResult> DeleteAsync([FromQuery] UserDeleteCommand userDeleteCommand)
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

        [Route("rm.auth")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAuthAsync([FromQuery] RemoveAuthorizationUserCommand removeAuthorizationUserCommand)
        {
            var result = await _mediator.Send(removeAuthorizationUserCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("edit.roles")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditRolesAsync([FromBody] EditUserRolesCommand editUserRolesCommand)
        {
            var result = await _mediator.Send(editUserRolesCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("edit.groups")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditGroupsAsync([FromBody] EditUserGroupsCommand editUserGroupsCommand)
        {
            var result = await _mediator.Send(editUserGroupsCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        #endregion
    }
}