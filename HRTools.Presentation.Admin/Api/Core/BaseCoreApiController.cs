using System;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Presentation.Admin.Helpers;

namespace HRTools.Presentation.Admin.Api.Core
{
    [AuthorizeTenant(Module = "Core")]
    public abstract class BaseCoreApiController : BaseApiController
    {
        protected BaseCoreApiController(Lazy<ILogger> logger) : base(logger)
        {
        }
    }
}
