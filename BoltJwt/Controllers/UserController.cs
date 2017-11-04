using System.Net;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        public UserController(IUserRepository userRepository)
        {

        }

        [HttpGet]
        [Authorize(Policy = "bJwtAdmins")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok();
        }
    }
}