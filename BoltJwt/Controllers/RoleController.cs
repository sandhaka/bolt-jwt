﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BoltJwt.Application.Commands.Roles;
using BoltJwt.Application.Queries.Roles;
using BoltJwt.Controllers.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IRoleQueries _roleQueries;

        public RoleController(IMediator mediator, IRoleQueries roleQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _roleQueries = roleQueries ?? throw new ArgumentNullException(nameof(roleQueries));
        }

        #region [ Queries ]

        [Route("")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAsync([FromUri] int id)
        {
            var result = await _roleQueries.GetAsync(id);

            return Ok(result);
        }

        [Route("all")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _roleQueries.GetAsync();

            return Ok(result);
        }

        [Route("paged")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(PagedData<IEnumerable<object>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PageQuery query)
        {
            var result = await _roleQueries.GetAsync(query);

            return Ok(result);
        }

        [Route("authorizations")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        public async Task<IActionResult> GetAuthAsync([FromUri] int id)
        {
            var result = await _roleQueries.GetAuthAsync(id);

            return Ok(result);
        }

        [Route("usage")]
        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsageAsync([FromUri] int id)
        {
            var result = await _roleQueries.GetUsageAsync(id);

            return Ok(result);
        }

        #endregion

        #region [ Commands ]

        [Route("")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] RoleInsertCommand roleInsertCommand)
        {
            var result = await _mediator.Send(roleInsertCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("update")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] RoleEditCommand roleEditCommand)
        {
            var result = await _mediator.Send(roleEditCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromQuery] RoleDeleteCommand roleDeleteCommand)
        {
            var result = await _mediator.Send(roleDeleteCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("add.auth")]
        [HttpPost]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAuthAsync([FromBody] AddAuthorizationRoleCommand addAuthorizationRoleCommand)
        {
            var result = await _mediator.Send(addAuthorizationRoleCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        [Route("rm.auth")]
        [HttpDelete]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAuthAsync([FromQuery] RemoveAuthorizationRoleCommand removeAuthorizationRoleCommand)
        {
            var result = await _mediator.Send(removeAuthorizationRoleCommand);

            return result ? Json(new { HttpStatusCode.OK }) : (IActionResult) BadRequest();
        }

        #endregion
    }
}