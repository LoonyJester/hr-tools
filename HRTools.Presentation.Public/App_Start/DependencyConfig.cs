using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using HRTools.Core.Common.Models;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Repositories.Company;
using HRTools.Core.Repositories.Employee;
using HRTools.Core.Repositories.JobTitle;
using HRTools.Core.Repositories.Technology;
using HRTools.Core.Services.Company;
using HRTools.Core.Services.Employee;
using HRTools.Core.Services.JobTitle;
using HRTools.Core.Services.Technology;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using HRTools.ProjectAssignment.Repositories.Assignment;
using HRTools.ProjectAssignment.Repositories.Department;
using HRTools.ProjectAssignment.Repositories.Project;
using HRTools.ProjectAssignment.Services.Assignment;
using HRTools.ProjectAssignment.Services.Department;
using HRTools.ProjectAssignment.Services.Project;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.WebApi;

namespace HRTools.Presentation.Public
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
        public static Container Container = new Container();

        public static void Resolve(Container container)
        {
            Container = container;

            Container.Options.LifestyleSelectionBehavior = new SingletonLifestyleSelectionBehavior();
            
            RegisterLogger();

            RegisterCrosscutting();
            
            RegisterCore();

            RegisterProjectAssignment();

            RegisterLazy();

            RegisterControllers();

            RegisterMapper();
            
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

        private static void RegisterCore()
        {
            Container.Register<IEmployeeService, EmployeeService>(Lifestyle.Transient);
            RegisterAutomatic(typeof(EmployeeRepository));
            RegisterAutomatic(typeof(CompanyService));
            RegisterAutomatic(typeof(CompanyRepository));
            RegisterAutomatic(typeof(JobTitleService));
            RegisterAutomatic(typeof(JobTitleRepository));
            RegisterAutomatic(typeof(TechnologyService));
            RegisterAutomatic(typeof(TechnologyRepository));
        }

        private static void RegisterProjectAssignment()
        {
            RegisterAutomatic(typeof(ProjectAssignment.Repositories.Employee.EmployeeRepository));
            RegisterAutomatic(typeof(ProjectAssignment.Services.Employee.EmployeeService));
            RegisterAutomatic(typeof(ProjectAssignmentRepository));
            RegisterAutomatic(typeof(ProjectAssignmentService));
            RegisterAutomatic(typeof(ProjectRepository));
            RegisterAutomatic(typeof(ProjectService));
            RegisterAutomatic(typeof(DepartmentRepository));
            RegisterAutomatic(typeof(DepartmentService));
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
            Container.RegisterLazy<ILogger>();
            Container.RegisterLazy<IEmployeeService>();
            Container.RegisterLazy<ICompanyService>();
            Container.RegisterLazy<IProjectAssignmentService>();
            Container.RegisterLazy<IProjectService>();
            Container.RegisterLazy<IProjectServiceAdmin>();
            Container.RegisterLazy<ProjectAssignment.Services.Employee.IEmployeeService>();
            Container.RegisterLazy<IJobTitleRepository>();
            Container.RegisterLazy<IJobTitleService>();
            Container.RegisterLazy<ITechnologyRepository>();
            Container.RegisterLazy<ITechnologyService>();
            Container.RegisterLazy<IDepartmentRepository>();
            Container.RegisterLazy<IDepartmentService>();
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
                    opts => opts.MapFrom(src => src.OfficeLocation.City));
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
                    }))
                ;

            cfg.CreateMap<OfficeLocationDto, OfficeLocation>();
            cfg.CreateMap<OfficeLocation, OfficeLocationDto>();
            cfg.CreateMap<JobTitle, JobTitleDto>();
            cfg.CreateMap<JobTitleDto, JobTitle>();
            cfg.CreateMap<Technology, TechnologyDto>();
            cfg.CreateMap<TechnologyDto, Technology>();
        }

        private static void RegisterMapperProjectAssignment(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ProjectAssignment.Common.Models.Assignment.ProjectAssignment, ProjectAssignmentDto>()
                .ForMember(x => x.EmployeeId,
                    opts => opts.MapFrom(src => src.Employee.EmployeeId))
                .ForMember(x => x.EmployeeFullName,
                    opts => opts.MapFrom(src => src.Employee.FullName))
                .ForMember(x => x.EmployeeJobTitle,
                    opts => opts.MapFrom(src => src.Employee.JobTitle))
                .ForMember(x => x.EmployeeTechnology,
                    opts => opts.MapFrom(src => src.Employee.Technology))
                .ForMember(x => x.ProjectId,
                    opts => opts.MapFrom(src => src.Project == null ? null : (int?) src.Project.Id))
                .ForMember(x => x.ProjectName,
                    opts => opts.MapFrom(src => src.Project == null ? null : src.Project.Name))
                .ForMember(x => x.DepartmentId,
                    opts => opts.MapFrom(src => src.Department.Id))
                .ForMember(x => x.DepartmentName,
                    opts => opts.MapFrom(src => src.Department.Name))
                ;

            cfg.CreateMap<ProjectAssignmentDto, ProjectAssignment.Common.Models.Assignment.ProjectAssignment>()
                .ForMember(x => x.Employee,
                    opts => opts.MapFrom(src => new ProjectAssignment.Common.Models.Core.Employee
                    {
                        EmployeeId = src.EmployeeId,
                        FullName = src.EmployeeFullName,
                        JobTitle = src.EmployeeJobTitle
                    }))
                .ForMember(x => x.Project,
                    opts =>
                        opts.MapFrom(src => src.ProjectId == null
                            ? null
                            : new ProjectAssignment.Common.Models.Project.Project
                            {
                                Id = (int) src.ProjectId,
                                Name = src.ProjectName
                            }))
                .ForMember(x => x.Department,
                    opts => opts.MapFrom(src => new ProjectAssignment.Common.Models.Department.Department
                    {
                        Id = src.DepartmentId,
                        Name = src.DepartmentName
                    }));

            cfg.CreateMap<ProjectDto, ProjectAssignment.Common.Models.Project.Project>();
            cfg.CreateMap<ProjectAssignment.Common.Models.Project.Project, ProjectDto>();

            cfg.CreateMap<DepartmentDto, ProjectAssignment.Common.Models.Department.Department>();
            cfg.CreateMap<ProjectAssignment.Common.Models.Department.Department, DepartmentDto>();

            cfg.CreateMap<ProjectAssignment.Repositories.Employee.EmployeeDto, ProjectAssignment.Common.Models.Core.Employee>();
            cfg.CreateMap<ProjectAssignment.Common.Models.Core.Employee, ProjectAssignment.Repositories.Employee.EmployeeDto>();
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