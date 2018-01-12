using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Core.Common.Models;
using HRTools.Core.Services.ModuleConfiguration;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Helpers;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Presentation.Admin.Api.Core
{
    public class ModulesConfigurationController : BaseCoreApiController
    {
        private readonly IModulesConfigurationServiceAdmin _modulesConfigurationServiceAdmin;

        public ModulesConfigurationController(IModulesConfigurationServiceAdmin modulesConfigurationServiceAdmin,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(modulesConfigurationServiceAdmin, nameof(modulesConfigurationServiceAdmin));

            _modulesConfigurationServiceAdmin = modulesConfigurationServiceAdmin;
        }

        [HttpGet]
        [Route("Api/GetModulesConfiguration")]
        public Task<HttpResponseMessage> GetAllAsync([FromUri] bool showOld = false, [FromUri] string companyName = null)
        {
            return TryExecuteAsync(async () =>
            {
                // todo: check
                Task<HttpResponseMessage> task;
                if (!ValidateUserRole(companyName, out task))
                {
                    return await task;
                }

                Guid clientId = companyName != null ? CompanyHelper.GetCompanyIdByHost(companyName) : ClientId;

                if (clientId == Guid.Empty)
                {
                    clientId = ClientId;
                }

                IEnumerable<ModuleConfig> moduleConfigList = await _modulesConfigurationServiceAdmin.GetAllAsync(clientId, showOld);

                return Request.CreateResponse(HttpStatusCode.OK, moduleConfigList);
            });
        }

        private bool ValidateUserRole(string companyName, out Task<HttpResponseMessage> task)
        {
            if (!string.IsNullOrEmpty(companyName) && CompanyName != companyName)
            {
                ClaimsPrincipal user = User as ClaimsPrincipal;
                Claim claim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Value == "SuperAdmin");

                if (claim == null)
                {
                    {
                        task = Task.FromResult(Request.CreateResponse(HttpStatusCode.Unauthorized));
                        return false;
                    }
                }
            }

            task = null;

            return true;
        }

        [HttpPost]
        [Route("Api/CreateModuleConfiguration")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] ModuleConfig moduleConfig)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(moduleConfig, nameof(moduleConfig));

                Guid clientId = moduleConfig.CompanyName != null ? CompanyHelper.GetCompanyIdByHost(moduleConfig.CompanyName) : ClientId;

                if (clientId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                moduleConfig.ClientId = clientId;

                List<ModuleConfig> actualModuleConfigs = (await _modulesConfigurationServiceAdmin.GetAllAsync(clientId, false)).ToList();

                int id = await _modulesConfigurationServiceAdmin.CreateAsync(moduleConfig, actualModuleConfigs, clientId);

                return Request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

        [HttpPut]
        [Route("Api/UpdateModuleConfiguration")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] ModuleConfig moduleConfig)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(moduleConfig, nameof(moduleConfig));

                Guid clientId = moduleConfig.CompanyName != null ? CompanyHelper.GetCompanyIdByHost(moduleConfig.CompanyName) : ClientId;

                if (clientId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                moduleConfig.ClientId = clientId;

                List<ModuleConfig> actualModuleConfigs = (await _modulesConfigurationServiceAdmin.GetAllAsync(clientId, false)).ToList();

                int updatedRows = await _modulesConfigurationServiceAdmin.UpdateAsync(moduleConfig, actualModuleConfigs, clientId);

                return Request.CreateResponse(HttpStatusCode.OK, updatedRows);
            });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        [Route("Api/StopAllModuleConfigurations")]
        public Task<HttpResponseMessage> StopAllAsync([FromBody] string companyName)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrEmpty(companyName, nameof(companyName));

                Guid clientId = CompanyHelper.GetCompanyIdByHost(companyName);

                if (clientId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                List<ModuleConfig> actualModuleConfigs = (await _modulesConfigurationServiceAdmin.GetAllAsync(clientId, false)).ToList();

                int updatedRows = await _modulesConfigurationServiceAdmin.StopAllAsync(actualModuleConfigs, clientId);

                return Request.CreateResponse(HttpStatusCode.OK, updatedRows);
            });
        }
    }
}
