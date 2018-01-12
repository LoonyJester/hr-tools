using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Models;
using HRTools.ProjectAssignment.Common.Models.Project;
using HRTools.ProjectAssignment.Services.Project;

namespace HRTools.Presentation.Admin.Api.ProjectAssignment
{
    public class ProjectController : BaseProjectAssignmentApiController
    {
        private readonly Lazy<IProjectServiceAdmin> _projectService;

        public ProjectController(
            Lazy<IProjectServiceAdmin> projectService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(projectService, nameof(projectService));

            _projectService = projectService;
        }

        private IProjectServiceAdmin ProjectService => _projectService.Value;

        [HttpGet]
        [Route("Api/GetAllProjects")]
        public Task<HttpResponseMessage> GetAllAsync(string gridSettings)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrWhiteSpace(gridSettings, nameof(gridSettings));

                ProjectGridSettings settings =
                    new JavaScriptSerializer().Deserialize<ProjectGridSettings>(gridSettings);

                GrigData<Project> projectGridData = await ProjectService.GetAllAsync(settings);

                return Request.CreateResponse(HttpStatusCode.OK, projectGridData);
            });
        }

        [HttpPost]
        [Route("Api/CreateProject")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] Project project)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(project, nameof(project));

                int id = await ProjectService.CreateAsync(project);

                return id == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Project was not successfully created")
                    : Request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

        [HttpPut]
        [Route("Api/UpdateProject")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] Project project)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(project, nameof(project));

                int updatedRowsCount = await ProjectService.UpdateAsync(project);

                return updatedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Project was not successfully updated")
                    : Request.CreateResponse(HttpStatusCode.OK, 1);
            });
        }

        [HttpDelete]
        [Route("Api/DeleteProject")]
        public Task<HttpResponseMessage> DeleteAsync([FromBody] int id)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(id, nameof(id));

                int deletedRowsCount = await ProjectService.DeleteAsync(id);

                return deletedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Project was not successfully deleted")
                    : Request.CreateResponse(HttpStatusCode.OK, 1);
            });
        }

        [HttpPut]
        [Route("Api/ActivateProject")]
        public Task<HttpResponseMessage> ActivateAsync(dynamic model)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(model, nameof(model));
                Guard.ArgumentIsNotNull(model.id, nameof(model.id));
                Guard.ArgumentIsNotNull(model.makeActive, nameof(model.makeActive));

                int id = Convert.ToInt32(model.id);
                bool makeActive = Convert.ToBoolean(model.makeActive);

                int wasActivated = await ProjectService.ActivateAsync(id, makeActive);

                return wasActivated == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, wasActivated)
                    : Request.CreateResponse(HttpStatusCode.OK, 1);
            });
        }
    }
}