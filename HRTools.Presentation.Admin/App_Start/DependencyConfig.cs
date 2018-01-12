using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using HRTools.Core.Common.Models;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Repositories.Company;
using HRTools.Core.Repositories.Employee;
using HRTools.Core.Repositories.JobTitle;
using HRTools.Core.Repositories.Module_Configuration;
using HRTools.Core.Repositories.Technology;
using HRTools.Core.Repositories.User;
using HRTools.Core.Services.Company;
using HRTools.Core.Services.Employee;
using HRTools.Core.Services.JobTitle;
using HRTools.Core.Services.ModuleConfiguration;
using HRTools.Core.Services.Technology;
using HRTools.Core.Services.User;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using HRTools.Crosscutting.Common.Models.User;
using HRTools.Crosscutting.Messaging;
using HRTools.Crosscutting.Messaging.Audit;
using HRTools.Crosscutting.Messaging.Events.Employee;
using HRTools.ProjectAssignment.Common.Models.Department;
using HRTools.ProjectAssignment.Common.Models.Project;
using HRTools.ProjectAssignment.Handlers.Employee;
using HRTools.ProjectAssignment.Repositories.Assignment;
using HRTools.ProjectAssignment.Repositories.Department;
using HRTools.ProjectAssignment.Repositories.Project;
using HRTools.ProjectAssignment.Services.Assignment;
using HRTools.ProjectAssignment.Services.Department;
using HRTools.ProjectAssignment.Services.Project;
using MassTransit;
using MassTransit.Audit;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.WebApi;
using ProjectAssignmentModels = HRTools.ProjectAssignment.Common.Models;

namespace HRTools.Presentation.Admin
{
    public static class ContainerExtensions
    {
        public static void RegisterLazy<T>(this Container container) where T : class
        {
            container.Register(() => new Lazy<T>(container.GetInstance<T>));
        }
    }

    public static class DependencyConfig
    {
        public static Container Container;

        public static void Resolve(Container container)
        {
            Container = container;

            Container.Options.LifestyleSelectionBehavior = new SingletonLifestyleSelectionBehavior();

            RegisterLogger();

            RegisterCrosscutting();
            
            RegisterBus();

            RegisterCore();

            RegisterProjectAssignment();

            RegisterLazy();

            RegisterControllers();

            RegisterMapper();

            //Container.Verify();

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

        private static void RegisterBus()
        {
            Container.Register<IEventPublisher, EventPublisher>();

            Container.Register<IConsumer<IEmployeeCreatedEvent>, CreateEmployeeConsumer>(Lifestyle.Transient);
            Container.Register<IConsumer<IEmployeeUpdatedEvent>, UpdateEmployeeConsumer>(Lifestyle.Transient);
            Container.Register<IConsumer<IEmployeeDeletedEvent>, DeleteEmployeeConsumer>(Lifestyle.Transient);

            Container.Register<IMessageAuditStore, MessageAuditStore>();
            Container.Register<IMessageAuditStoreService, MessageAuditStoreService>();
            Container.Register<IMessageAuditStoreRepository, MessageAuditStoreRepository>();
        }

        private static void RegisterCore()
        {
            RegisterAutomatic(typeof(EmployeeRepository));
            RegisterAutomatic(typeof(EmployeeServiceAdmin));
            RegisterAutomatic(typeof(CompanyRepository));
            RegisterAutomatic(typeof(CompanyService));
            RegisterAutomatic(typeof(UserRepository));
            RegisterAutomatic(typeof(UserService));
            RegisterAutomatic(typeof(JobTitleRepository));
            RegisterAutomatic(typeof(JobTitleService));
            RegisterAutomatic(typeof(TechnologyRepository));
            RegisterAutomatic(typeof(TechnologyService));
            RegisterAutomatic(typeof(ModulesConfigurationServiceAdmin));

            // TODO: refactor
            Container.Register<IModulesConfigurationRepositoryAdmin>(() => new ModulesConfigurationRepositoryAdmin(new MasterConnectionFactory(), new ConfigurationManager(new CacheConfigurationRepository(), new DbConfigurationRepository(new MasterConnectionFactory(), new MySqlNLogLogger())), new MySqlNLogLogger()));
            //RegisterAutomatic(typeof(ModulesConfigurationRepositoryAdmin));
        }

        private static void RegisterProjectAssignment()
        {
            RegisterAutomatic(typeof(ProjectAssignmentRepository));
            RegisterAutomatic(typeof(ProjectAssignmentService));
            RegisterAutomatic(typeof(ProjectRepository));
            RegisterAutomatic(typeof(ProjectService));
            RegisterAutomatic(typeof(DepartmentRepository));
            RegisterAutomatic(typeof(DepartmentService));
            RegisterAutomatic(typeof(ProjectAssignment.Repositories.Employee.EmployeeRepository));
            RegisterAutomatic(typeof(ProjectAssignment.Services.Employee.EmployeeService));
        }

        private static void RegisterAutomatic(Type type)
        {
            var registrations = type.Assembly.GetExportedTypes()
                .Where(x => x.Namespace == type.Namespace)
                .Select(
                    x => new
                    {
                        Services = x.GetInterfaces(),
                        Implementation = x
                    })
                .Where(x => x.Services.Any())
                .Where(x => !x.Implementation.IsAbstract);

            foreach (var registration in registrations)
            {
                foreach (Type service in registration.Services)
                {
                    Container.Register(service, registration.Implementation, Lifestyle.Transient);
                }
            }
        }

        private static void RegisterLazy()
        {
            RegisterLazyCore();

            RegisterLazyProjectAssignment();
        }

        private static void RegisterLazyCore()
        {
            Container.RegisterLazy<ILogger>();
            Container.RegisterLazy<IEmployeeServiceAdmin>();
            Container.RegisterLazy<ICompanyServiceAdmin>();
            Container.RegisterLazy<IUserServiceAdmin>();
            Container.RegisterLazy<IJobTitleServiceAdmin>();
            Container.RegisterLazy<ITechnologyServiceAdmin>();
        }

        private static void RegisterLazyProjectAssignment()
        {
            Container.RegisterLazy<IProjectAssignmentService>();
            Container.RegisterLazy<IProjectServiceAdmin>();
            Container.RegisterLazy<IDepartmentServiceAdmin>();
        }

        private static void RegisterControllers()
        {
            Container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
        }

        private static void RegisterMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                RegisterMapperCore(cfg);

                RegisterMapperProjectAssignment(cfg);
            });

            Container.RegisterSingleton(mapperConfiguration.CreateMapper);
        }

        private static void RegisterMapperCore(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Employee, EmployeeDto>()
                .ForMember(x => x.Birthday,
                    opts => opts.MapFrom(src => src.Birthday == null ? (DateTime?) null : src.Birthday.Value.ToUniversalTime()))
                .ForMember(x => x.StartDate,
                    opts => opts.MapFrom(src => src.StartDate == null ? (DateTime?) null : src.StartDate.ToUniversalTime()))
                .ForMember(x => x.TerminationDate,
                    opts =>
                        opts.MapFrom(
                            src => src.TerminationDate == null ? (DateTime?) null : src.TerminationDate.Value.ToUniversalTime()))
                .ForMember(x => x.Country,
                    opts => opts.MapFrom(src => src.OfficeLocation.Country))
                .ForMember(x => x.City,
                    opts => opts.MapFrom(src => src.OfficeLocation.City))
                .ForMember(x => x.MessengerName,
                    opts => opts.MapFrom(src => src.Messenger.Name))
                .ForMember(x => x.MessengerLogin,
                    opts => opts.MapFrom(src => src.Messenger.Login))
                ;
            cfg.CreateMap<EmployeeDto, Employee>()
                .ForMember(x => x.OfficeLocation,
                    opts => opts.MapFrom(src => new OfficeLocation
                    {
                        Country = src.Country,
                        City = src.City
                    }))
                .ForMember(x => x.Messenger,
                    opts => opts.MapFrom(src => new Messenger
                    {
                        Name = src.MessengerName,
                        Login = src.MessengerLogin
                    }));

            cfg.CreateMap<OfficeLocationDto, OfficeLocation>();
            cfg.CreateMap<OfficeLocation, OfficeLocationDto>();
            cfg.CreateMap<User, UserDto>()
                .ForMember(x => x.Subject,
                    opts => opts.MapFrom(src => src.UserId));
            cfg.CreateMap<UserDto, User>()
                .ForMember(x => x.UserId,
                    opts => opts.MapFrom(src => src.Subject));

            cfg.CreateMap<JobTitleDto, JobTitle>();
            cfg.CreateMap<JobTitle, JobTitleDto>();
            cfg.CreateMap<TechnologyDto, Technology>();
            cfg.CreateMap<Technology, TechnologyDto>();
            cfg.CreateMap<ModuleConfigDto, ModuleConfig>();
            cfg.CreateMap<ModuleConfig, ModuleConfigDto>();
        }

        private static void RegisterMapperProjectAssignment(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ProjectDto, Project>();
            cfg.CreateMap<Project, ProjectDto>();

            cfg.CreateMap<DepartmentDto, Department>();
            cfg.CreateMap<Department, DepartmentDto>();

            cfg.CreateMap<ProjectAssignment.Repositories.Employee.EmployeeDto, ProjectAssignmentModels.Core.Employee>();
            cfg.CreateMap<ProjectAssignmentModels.Core.Employee, ProjectAssignment.Repositories.Employee.EmployeeDto>();
        }

        public static void ResolveBus(IBusControl busControl)
        {
            Container.RegisterSingleton<IPublishEndpoint>(busControl);
        }

        public static void Verify()
        {
            Container.Verify();
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