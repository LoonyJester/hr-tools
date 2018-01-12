using System;
using System.Web;
using System.Web.Http;
using HRTools.Crosscutting.Common.Helpers;
using HRTools.Crosscutting.Common.Master;
using HRTools.Presentation.Admin.App_Start;
using MassTransit;
using SimpleInjector;

namespace HRTools.Presentation.Admin
{
    public class Global : HttpApplication
    {
        static IBusControl _busControl;

        private static readonly Container Container = new Container();

        void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyConfig.Resolve(Container);
            SerializationConfig.ConfigureJsonSerializer();

            MessageBusConfig.Configure(Container, out _busControl);
            DependencyConfig.ResolveBus(_busControl);

            DependencyConfig.Verify();

            MessageBusConfig.ConfigureWhenContainerIsVerified(Container, _busControl);

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

        protected void Application_End()
        {
            _busControl.Stop(TimeSpan.FromSeconds(10)); ;
        }
    }
}