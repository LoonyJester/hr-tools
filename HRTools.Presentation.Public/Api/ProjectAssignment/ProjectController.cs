using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.ProjectAssignment.Common.Models.Project;
using HRTools.ProjectAssignment.Services.Project;

namespace HRTools.Presentation.Public.Api.ProjectAssignment
{
    public class ProjectController : BaseProjectAssignmentApiController
    {
        private readonly Lazy<IProjectService> _projectService;

        public ProjectController(
            Lazy<IProjectService> projectService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(projectService, nameof(projectService));

            _projectService = projectService;
        }

        private IProjectService ProjectService => _projectService.Value;

        [HttpGet]
        [Route("Api/GetProjectsByNameAutocomplete")]
        public Task<HttpResponseMessage> GetAllAsync(string nameAutocomplete, bool showDeactivated, bool showOld)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrWhiteSpace(nameAutocomplete, nameof(nameAutocomplete));

                IEnumerable<Project> projectGridData = await ProjectService.GetProjectsByNameAutocompleteAsync(nameAutocomplete, showDeactivated, showOld);

                return Request.CreateResponse(HttpStatusCode.OK, projectGridData);
            });
        }
    }
}