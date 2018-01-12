using System;
using System.Web.Http;
using HRTools.AuthorizationServer.Services;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.WebApi;
using Container = SimpleInjector.Container;

namespace HRTools.AuthorizationServer
{
    public static class ContainerExt
    {
        public static void RegisterLazy<T>(this Container container) where T : class
        {
            container.Register(() => new Lazy<T>(container.GetInstance<T>));
        }
    }

    public static class DependencyConfig
    {
        internal static readonly Container Container = new Container();

        public static void Resolve()
        {
            Container.Options.LifestyleSelectionBehavior = new SingletonLifestyleSelectionBehavior();

            RegisterLogger();

            RegisterCrosscutting();
            
            Container.Register<IAuthService, AuthService>(Lifestyle.Singleton);

            RegisterLazy();

            RegisterControllers();
            
            Container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);
        }

        private static void RegisterLogger()
        {
            Container.RegisterSingleton<ILogger>(new MySqlNLogLogger());
        }

        private static void RegisterCrosscutting()
        {
            Container.Register<ICacheConfigurationRepository, CacheConfigurationRepository>();
            Container.Register<IConfigurationManager, ConfigurationManager>();
            Container.Register<IDbConfigurationRepository>(() => new DbConfigurationRepository(new MasterConnectionFactory(), new MySqlNLogLogger()));
            Container.Register<IConnectionFactory, DefaultConnectionFactory>();
        }

        private static void RegisterLazy()
        {
            Container.RegisterLazy<ILogger>();
            //Container.RegisterLazy<IAuthService>();
        }

        private static void RegisterControllers()
        {
            Container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
        }
        
        private class SingletonLifestyleSelectionBehavior : ILifestyleSelectionBehavior
        {
            public Lifestyle SelectLifestyle(Type serviceType, Type implementationType)
            {
                return Lifestyle.Singleton;
            }
        }
    }
}