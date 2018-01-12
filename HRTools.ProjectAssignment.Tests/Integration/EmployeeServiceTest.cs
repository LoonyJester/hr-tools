using AutoMapper;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using HRTools.ProjectAssignment.Repositories.Assignment;
using HRTools.ProjectAssignment.Repositories.Employee;
using HRTools.ProjectAssignment.Services.Employee;
using Moq;
using NUnit.Framework;

namespace HRTools.ProjectAssignment.Tests.Integration
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        private IEmployeeService _employeeService;
        private IEmployeeRepository _employeeRepository;

        [SetUp]
        public void SetUp()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.CreateMap<Common.Models.Assignment.ProjectAssignment, ProjectAssignmentDto>();
                cfg.CreateMap<ProjectAssignmentDto, Common.Models.Assignment.ProjectAssignment>();
            });
            
            Mock<IConfigurationManager> configurationManagerMock = new Mock<IConfigurationManager>();
            
            DefaultConnectionFactory defaultConnectionFactory = new DefaultConnectionFactory();
            _employeeRepository = new EmployeeRepository(defaultConnectionFactory, configurationManagerMock.Object, new MySqlNLogLogger());
            _employeeService = new EmployeeService(_employeeRepository, new Mapper(mapperConfiguration));
        }

        #region Public

        #endregion
    }
}
