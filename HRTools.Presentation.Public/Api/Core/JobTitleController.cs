using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Core.Common.Models;
using HRTools.Core.Services.JobTitle;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Presentation.Public.Api.Core
{
    public class JobTitleController : BaseCoreApiController
    {
        private readonly Lazy<IJobTitleService> _jobTitleService;

        public JobTitleController(
            Lazy<IJobTitleService> jobTitleService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(jobTitleService, nameof(jobTitleService));

            _jobTitleService = jobTitleService;
        }

        private IJobTitleService JobTitleService => _jobTitleService.Value;

        [HttpGet]
        [Route("Api/GetAllJobTitles")]
        public Task<HttpResponseMessage> GetAllAsync()
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<JobTitle> jobTitleList = await JobTitleService.GetAllAsync();

                return Request.CreateResponse(HttpStatusCode.OK, jobTitleList);
            });
        }
    }
}