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

namespace HRTools.Presentation.Admin.Api.Core
{
    public class JobTitleController : BaseCoreApiController
    {
        private readonly Lazy<IJobTitleServiceAdmin> _jobTitleService;

        public JobTitleController(
            Lazy<IJobTitleServiceAdmin> projectService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(projectService, nameof(projectService));

            _jobTitleService = projectService;
        }

        private IJobTitleServiceAdmin JobTitleService => _jobTitleService.Value;

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

        [HttpPost]
        [Route("Api/CreateJobTitle")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] JobTitle jobTitle)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(jobTitle, nameof(jobTitle));

                int id = await JobTitleService.CreateAsync(jobTitle);

                return id == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Job title was not successfully created")
                    : Request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

        [HttpPut]
        [Route("Api/UpdateJobTitle")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] JobTitle jobTitle)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(jobTitle, nameof(jobTitle));

                int updatedRowsCount = await JobTitleService.UpdateAsync(jobTitle);

                return updatedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Job title was not successfully updated")
                    : Request.CreateResponse(HttpStatusCode.OK, updatedRowsCount);
            });
        }

        [HttpDelete]
        [Route("Api/DeleteJobTitle")]
        public Task<HttpResponseMessage> DeleteAsync([FromBody] int id)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(id, nameof(id));

                int deletedRowsCount = await JobTitleService.DeleteAsync(id);

                return deletedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Job title was not successfully deleted")
                    : Request.CreateResponse(HttpStatusCode.OK, deletedRowsCount);
            });
        }
    }
}