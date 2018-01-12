using System;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using HRTools.AuthorizationServer;
using HRTools.AuthorizationServer.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SimpleInjector.Integration.WebApi;

[assembly: OwinStartup(typeof(Startup))]

namespace HRTools.AuthorizationServer
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureCors(app);
            ConfigureOAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(DependencyConfig.Container);

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

            policy.Origins.Add("http://teaminternational:3001");
            policy.Origins.Add("http://company:3001");

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

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions()
            {
                
            };

            var provider = DependencyConfig.Container.GetInstance(typeof(AuthorizationServerProvider)) as AuthorizationServerProvider;
            var refreshTokenProvider = DependencyConfig.Container.GetInstance(typeof(RefreshTokenProvider)) as RefreshTokenProvider;

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(480),
                Provider = provider,//new AuthorizationServerProvider(),
                RefreshTokenProvider = refreshTokenProvider//new RefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}