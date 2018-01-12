using System;
using System.Web;
using System.Web.Http;
using HRTools.Crosscutting.Common.Helpers;
using HRTools.Crosscutting.Common.Master;
using SimpleInjector;

namespace HRTools.Presentation.Public
{
    public class Global : HttpApplication
    {
        private static readonly Container Container = new Container();
        
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyConfig.Resolve(Container);
            SerializationConfig.ConfigureJsonSerializer();

            CreateConfigurationInCache(Container);
        }

        private static void CreateConfigurationInCache(Container container)
        {
            IConfigurationManager configurationManager = container.GetInstance<IConfigurationManager>();

            configurationManager.CreateConfigurationInCacheAsync(); 
        }
        
        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            string host = HttpContext.Current.Request.Url.Host;
            Guid companyId = CompanyHelper.GetCompanyIdByHost(host);

            HttpContext.Current.Request.Headers.Add("CompanyId", companyId.ToString());
        }
    }
}