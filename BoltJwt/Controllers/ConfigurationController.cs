using System.Net;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
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
            var config = await _configurationRepository.GetAsync();

            // TODO: Move to application command and add dto validation
            config.UpdateInfo(new Configuration
            {
                SmtpHostName = dto.SmtpHostName,
                SmtpPort = dto?.SmtpPort ?? 0,
                SmtpUserName = dto.SmtpUserName,
                SmtpPassword = dto.SmtpPassword,
                SmtpEmail = dto.SmtpEmail,
                EndpointFqdn = dto.EndpointFqdn,
                EndpointPort = dto?.EndpointPort ?? 0,
                RootPassword = dto.RootPassword
            });

            var result = await _configurationRepository.UnitOfWork.SaveEntitiesAsync();

            return Json(new {HttpStatusCode.OK, result});
        }
    }
}