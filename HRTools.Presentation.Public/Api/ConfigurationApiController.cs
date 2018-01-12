using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Presentation.Public.Api
{
    public class ConfigurationApiController : BaseApiController
    {
        private readonly IConfigurationManager _configurationManager;

        public ConfigurationApiController(IConfigurationManager configurationManager,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(configurationManager, nameof(configurationManager));

            _configurationManager = configurationManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Api/GetActiveModules")]
        public Task<HttpResponseMessage> GetActiveModulesAsync()
        {
            return TryExecuteAsync(async () =>
            {
                ClaimsPrincipal user = User as ClaimsPrincipal;

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                ClientConfiguration configuration = await _configurationManager.GetConfigurationAsync();

                return Request.CreateResponse(HttpStatusCode.OK, configuration.ActiveModules);
            });
        }
    }
}
