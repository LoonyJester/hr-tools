using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SimpleInjector.Integration.WebApi;

[assembly: OwinStartup(typeof(HRTools.Presentation.Admin.Startup))]

namespace HRTools.Presentation.Admin
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        static bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            // Ignore errors
            return true;
        }

        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;

        //    ConfigureCors(app);

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(DependencyConfig.Container);
            //config.Filters.Add(new AuthorizeAttribute());

       //     ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseWebApi(config);

        }

        private static void ConfigureCors(IAppBuilder app)
        {
            CorsPolicy policy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = false,
                SupportsCredentials = true
            };

            policy.Origins.Add("http://teaminternational.admin:3000");
            policy.Origins.Add("http://company.admin:3000");

            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            //Token Consumption
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}