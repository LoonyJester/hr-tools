using System;
using System.Web;
using System.Web.Http;
using HRTools.Crosscutting.Common.Helpers;

namespace HRTools.AuthorizationServer
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyConfig.Resolve();
        }

        protected void Application_BeginRequest()
        {
            string host = HttpContext.Current.Request.Url.Host;
            Guid companyId = CompanyHelper.GetCompanyIdByHost(host);

            HttpContext.Current.Request.Headers.Add("CompanyId", companyId.ToString());
        }
    }
}
