using System;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Presentation.Admin.Helpers;

namespace HRTools.Presentation.Admin.Api.ProjectAssignment
{
    [AuthorizeTenant(Module = "Project Assignment")]
    public abstract class BaseProjectAssignmentApiController : BaseApiController
    {
        protected BaseProjectAssignmentApiController(Lazy<ILogger> logger) : base(logger)
        {
        }
    }
}
