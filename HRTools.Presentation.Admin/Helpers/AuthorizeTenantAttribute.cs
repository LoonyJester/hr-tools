using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Presentation.Admin.Helpers
{
    public class AuthorizeTenantAttribute: AuthorizeAttribute
    {
        public string Module { get; set; }

        public override async void OnAuthorization(HttpActionContext filterContext)
        {
            base.OnAuthorization(filterContext);

            IConfigurationManager configurationManager = DependencyConfig.Container.GetInstance<IConfigurationManager>();
            
            ClientConfiguration configuration = await configurationManager.GetConfigurationAsync();

            if (!configuration.ActiveModules.Contains(Module))
            {
                filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"You don't have an access to the {Module} module. Please, update a subscription or contact your administrator.");
            }
        }
    }
}