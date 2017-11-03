using System.Threading.Tasks;
using BoltJwt.Model.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Policy = "BoltJwtAdmin")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            return Ok(user);
        }
    }
}