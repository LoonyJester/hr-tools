using System;
using System.Web.Http;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Presentation.Public.Helpers;

namespace HRTools.Presentation.Public.Api.ProjectAssignment
{
    [AuthorizeTenant(Module = "Project Assignment")]
    [Authorize(Roles = "Manager")]
    public abstract class BaseProjectAssignmentApiController : BaseApiController
    {
        protected BaseProjectAssignmentApiController(Lazy<ILogger> logger) : base(logger)
        {
        }
    }
}
