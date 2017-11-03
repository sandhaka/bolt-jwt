using System.Net;
using System.Threading.Tasks;
using BoltJwt.Model;
using BoltJwt.Model.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("user")]
        [Authorize(Policy = "BoltJwtAdmin")]
        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            return user != null ? Ok(user) : (IActionResult) BadRequest();
        }
    }
}