using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Core.Common.Models;
using HRTools.Core.Services.Technology;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Presentation.Public.Api.Core
{
    public class TechnologyController : BaseCoreApiController
    {
        private readonly Lazy<ITechnologyService> _tehcnologyService;

        public TechnologyController(
            Lazy<ITechnologyService> tehcnologyService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(tehcnologyService, nameof(tehcnologyService));

            _tehcnologyService = tehcnologyService;
        }

        private ITechnologyService TehcnologyService => _tehcnologyService.Value;

        [HttpGet]
        [Route("Api/GetAllTechnologies")]
        public Task<HttpResponseMessage> GetAllAsync()
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<Technology> jobTitleList = await TehcnologyService.GetAllAsync();

                return Request.CreateResponse(HttpStatusCode.OK, jobTitleList);
            });
        }
    }
}