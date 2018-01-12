using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Models;
using HRTools.ProjectAssignment.Common.Models.Assignment;
using HRTools.ProjectAssignment.Services.Assignment;
using Models = HRTools.ProjectAssignment.Common.Models;

namespace HRTools.Presentation.Public.Api.ProjectAssignment
{
    public class ProjectAssignmentController : BaseProjectAssignmentApiController
    {
        private readonly Lazy<IProjectAssignmentService> _projectAssignmentService;

        public ProjectAssignmentController(
            Lazy<IProjectAssignmentService> projectAssignmentService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(projectAssignmentService, nameof(projectAssignmentService));

            _projectAssignmentService = projectAssignmentService;
        }

        private IProjectAssignmentService ProjectAssignmentService => _projectAssignmentService.Value;

        [HttpGet]
        [Route("Api/GetAllProjectAssignments")]
        public Task<HttpResponseMessage> GetAllAsync(string gridSettings)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrWhiteSpace(gridSettings, nameof(gridSettings));

                ProjectAssignmentGridSettings settings =
                    new JavaScriptSerializer().Deserialize<ProjectAssignmentGridSettings>(gridSettings);

                GrigData<Models.Assignment.ProjectAssignment> projectAssignmentGridData =
                    await ProjectAssignmentService.GetAllAsync(settings);

                return Request.CreateResponse(HttpStatusCode.OK, projectAssignmentGridData);
            });
        }

        [HttpPost]
        [Route("Api/CreateProjectAssignment")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] Models.Assignment.ProjectAssignment projectAssignment)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(projectAssignment, nameof(projectAssignment));

                int id = await ProjectAssignmentService.CreateAsync(projectAssignment);

                return id == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Project Assignment was not successfully created")
                    : Request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

        [HttpPut]
        [Route("Api/UpdateProjectAssignment")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] Models.Assignment.ProjectAssignment projectAssignment)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(projectAssignment, nameof(projectAssignment));

                int updatedRowsCount = await ProjectAssignmentService.UpdateAsync(projectAssignment);

                return updatedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Project Assignment was not successfully updated")
                    : Request.CreateResponse(HttpStatusCode.OK, updatedRowsCount);
            });
        }

        [HttpDelete]
        [Route("Api/DeleteProjectAssignment")]
        public Task<HttpResponseMessage> DeleteAsync([FromBody]int id)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(id, nameof(id));

                int deletedRowsCount = await ProjectAssignmentService.DeleteAsync(id);

                return deletedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Project Assignment was not successfully deleted")
                    : Request.CreateResponse(HttpStatusCode.OK, deletedRowsCount);
            });
        }
    }
}