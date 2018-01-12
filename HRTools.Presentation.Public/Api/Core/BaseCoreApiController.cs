using System;
using System.Web.Http;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Presentation.Public.Helpers;

namespace HRTools.Presentation.Public.Api.Core
{
    [AuthorizeTenant(Module = "Core")]
    [Authorize(Roles = "Employee,Manager")]
    public abstract class BaseCoreApiController : BaseApiController
    {
        protected BaseCoreApiController(Lazy<ILogger> logger) : base(logger)
        {
        }
    }
}
