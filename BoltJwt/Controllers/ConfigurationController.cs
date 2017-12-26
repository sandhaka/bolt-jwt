using System.Net;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Infrastructure.AppConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers
{
    [Route("api/v1/[controller]")]
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationController(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        [Route("")]
        [HttpGet]
        [Authorize(Policy = "bJwtRoot")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _configurationRepository.GetAsync();

            return Json(new {HttpStatusCode.OK, config = result});
        }

        [Route("")]
        [HttpPost]
        [Authorize(Policy = "bJwtRoot")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SaveAsync([FromBody] ConfigurationDto dto)
        {
            var result = await _configurationRepository.UpdateAsync(dto);

            await _configurationRepository.UnitOfWork.SaveEntitiesAsync();

            return Json(new {HttpStatusCode.OK, result});
        }
    }
}