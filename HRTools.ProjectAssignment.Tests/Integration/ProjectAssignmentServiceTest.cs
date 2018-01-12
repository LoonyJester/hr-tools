//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Transactions;
//using System.Web.Script.Serialization;
//using AutoMapper;
//using HRTools.Crosscutting.Common.DataAccess;
//using HRTools.Crosscutting.Common.Logging;
//using HRTools.Crosscutting.Common.Master;
//using HRTools.Crosscutting.Common.Models;
//using HRTools.ProjectAssignment.Common.Models.Assignment;
//using HRTools.ProjectAssignment.Common.Models.Core;
//using HRTools.ProjectAssignment.Common.Models.Department;
//using HRTools.ProjectAssignment.Common.Models.Project;
//using HRTools.ProjectAssignment.Repositories.ProjectAssignment.Assignment;
//using HRTools.ProjectAssignment.Services.ProjectAssignment.Assignment;
//using NUnit.Framework;

//namespace HRTools.ProjectAssignment.Tests.Integration
//{
//    [TestFixture]
//    public class ProjectAssignmentServiceTest
//    {
//        private IProjectAssignmentServiceAdmin _projectAssignmentServiceAdmin;
//        private IProjectAssignmentRepositoryAdmin _projectAssignmentRepository;

//        [SetUp]
//        public void SetUp()
//        {
//            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => {
//                cfg.CreateMap<Common.Models.Assignment.ProjectAssignment, ProjectAssignmentDto>();
//                cfg.CreateMap<ProjectAssignmentDto, Common.Models.Assignment.ProjectAssignment>();
//            });
            
//            const string connectionName = "MySqlConnectionString";
//            DefaultConnectionFactory defaultConnectionFactory = new DefaultConnectionFactory(new MasterRepository(new MasterConnectionFactory(connectionName), new MySqlNLogLogger()));
//            _projectAssignmentRepository = new ProjectAssignmentRepository(defaultConnectionFactory, new MySqlNLogLogger());
//            _projectAssignmentServiceAdmin = new ProjectAssignmentService(_projectAssignmentRepository, new Mapper(mapperConfiguration));
//        }

//        #region Admin - GetAllAsync

//        [Test]
//        public async void Admin_GetAllAsync_EmptyFilters_AllEmployees()
//        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();

//            using (TransactionScope scope = new TransactionScope())
//            {
//                ProjectAssignmentGridSettings defaultGridSettings = BuildDefaultGridSettings();

//                Employee employee1 = BuildMinValidEmployee();
//                Employee employee2 = BuildMinValidEmployee();
//                Employee employee3 = BuildMinValidEmployee();

//                List<Employee> expected = new List<Employee> {employee1, employee2, employee3}.OrderBy(x => x.FullName).ToList();

//                // Act
//                Guid? id1 = await _projectAssignmentServiceAdmin.CreateAsync(employee1);
//                employee1.EmployeeId = id1.Value;
//                Guid? id2 = await _projectAssignmentServiceAdmin.CreateAsync(employee2);
//                employee2.EmployeeId = id2.Value;
//                Guid? id3 = await _projectAssignmentServiceAdmin.CreateAsync(employee3);
//                employee3.EmployeeId = id3.Value;

//                GrigData<Employee> employeesGrigData = await _projectAssignmentServiceAdmin.GetAllAsync(defaultGridSettings);
               
//                // Assert
//                Assert.AreEqual(3, employeesGrigData.TotalCount);
//                Assert.AreEqual(serializer.Serialize(expected), 
//                    serializer.Serialize(employeesGrigData.Data.ToList()));
//            }
//        }

//        [Test]
//        public async void Admin_GetAllAsync_SearchFiltersByAllFields_SearchedAndFilteredEmployees()
//        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();

//            using (TransactionScope scope = new TransactionScope())
//            {
//                EmployeeGridSettings defaultGridSettings = new EmployeeGridSettings
//                {
//                    PagingSettings = new PagingSettings
//                    {
//                        CurrentPage = "1",
//                        ItemsPerPage = "2"
//                    },
//                    SortingSettings = new SortingSettings
//                    {
//                        IsDescending = false,
//                        SortColumnName = "FullName"
//                    },
//                    EmployeeFilter = new EmployeeFilter
//                    {
//                        City = "Kharkiv",
//                        Country = "Ukraine",
//                        Status = EmployeeStatus.Hired,
//                        AllCitiesText = "All Cities",
//                        AllCountriesText = "All Countries"
//                    },
//                    SearchKeyword = "employee"
//                };

//                Employee employee1 = BuildMinValidEmployee();
//                employee1.FullName = "employee 1";
//                employee1.OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Kharkiv"
//                };
//                employee1.Status = (int)EmployeeStatus.Hired;

//                Employee employee2 = BuildMinValidEmployee();
//                employee2.FullName = "1";
//                employee2.JobTitle = "employee";
//                employee2.OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Kharkiv"
//                };
//                employee2.Status = (int)EmployeeStatus.Hired;

//                Employee employee3 = BuildMinValidEmployee();
//                employee3.FullName = "2";
//                employee3.JobTitle = "3";
//                employee3.DepartmentName = "employee";
//                employee3.OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Kharkiv"
//                };
//                employee3.Status = (int)EmployeeStatus.Hired;

//                Employee employee4 = BuildMinValidEmployee();
//                employee4.FullName = "2";
//                employee4.JobTitle = "3";
//                employee4.DepartmentName = "4";
//                employee4.OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Lviv"
//                };
//                employee4.Status = (int)EmployeeStatus.Hired;

//                Employee employee5 = BuildMinValidEmployee();
//                employee5.FullName = "2";
//                employee5.JobTitle = "3";
//                employee5.DepartmentName = "4";
//                employee5.OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Kharkiv"
//                };
//                employee5.Status = (int)EmployeeStatus.Dismissed;

//                List<Employee> expected = new List<Employee> {employee1, employee2, employee3}.OrderBy(x => x.FullName).Take(2).ToList();

//                // Act
//                Guid? id1 = await _projectAssignmentServiceAdmin.CreateAsync(employee1);
//                employee1.EmployeeId = id1.Value;
//                Guid? id2 = await _projectAssignmentServiceAdmin.CreateAsync(employee2);
//                employee2.EmployeeId = id2.Value;
//                Guid? id3 = await _projectAssignmentServiceAdmin.CreateAsync(employee3);
//                employee3.EmployeeId = id3.Value;
//                Guid? id4 = await _projectAssignmentServiceAdmin.CreateAsync(employee4);
//                employee4.EmployeeId = id4.Value;
//                Guid? id5 = await _projectAssignmentServiceAdmin.CreateAsync(employee5);
//                employee5.EmployeeId = id5.Value;

//                GrigData<Employee> employeesGrigData = await _projectAssignmentServiceAdmin.GetAllAsync(defaultGridSettings);
               
//                // Assert
//                Assert.AreEqual(3, employeesGrigData.TotalCount);
//                Assert.AreEqual(2, employeesGrigData.Data.Count());
//                Assert.AreEqual(serializer.Serialize(expected), 
//                    serializer.Serialize(employeesGrigData.Data.ToList()));
//            }
//        }

//        #endregion

//        #region Admin - Create

//        [Test]
//        public async void Admin_CreateAsync_MinValidEmployee_Created()
//        {
//            // Arrange
//            JavaScriptSerializer serializer = new JavaScriptSerializer();

//            using (TransactionScope scope = new TransactionScope())
//            {
//                Employee employee = BuildMinValidEmployee();

//                // Act
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);

//                Employee actualEmployee = await _projectAssignmentServiceAdmin.GetByIdAsync(id.Value);
//                employee.EmployeeId = id.Value;
//                string expected = serializer.Serialize(employee);
//                string actual = serializer.Serialize(actualEmployee);

//                // Assert
//                Assert.IsNotNull(id);
//                Assert.AreEqual(expected, actual);
//            }
//        }

//        [Test]
//        public async void Admin_CreateAsync_FilledValidEmployee_Created()
//        {
//            // Arrange
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//            using (TransactionScope scope = new TransactionScope())
//            {
//                Employee employee = BuildFilledValidEmployee();

//                // Act
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);

//                Employee actualEmployee = await _projectAssignmentServiceAdmin.GetByIdAsync(id.Value);
//                employee.EmployeeId = id.Value;
//                string expected = serializer.Serialize(employee);
//                string actual = serializer.Serialize(actualEmployee);

//                // Assert
//                Assert.IsNotNull(id);
//                Assert.AreEqual(expected, actual);
//            }
//        }

//        [Test]
//        public async void Admin_CreateAsync_SameAsExistedEmployee_Created()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = BuildMinValidEmployee();
//                Employee sameEmployee = BuildMinValidEmployee();

//                // Act
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);
//                Guid? newId = await _projectAssignmentServiceAdmin.CreateAsync(sameEmployee);

//                // Assert
//                Assert.IsNotNull(id);
//                Assert.IsNotNull(newId);
//                Assert.AreNotEqual(id, newId);
//            }
//        }

//        [Test]
//        // Assert
//        [ExpectedException(typeof(ValidationException))]
//        public async void Admin_CreateAsync_EmptyEmployee_ValidationException()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = new Employee();

//                // Act
//                await _projectAssignmentServiceAdmin.CreateAsync(employee);
//            }
//        }

//        [Test]
//        // Assert
//        [ExpectedException(typeof(ValidationException))]
//        public async void Admin_CreateAsync_NotValidEmail_ValidationException()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = BuildMinValidEmployee();
//                employee.CompanyEmail = "notValidEmail";

//                // Act
//                await _projectAssignmentServiceAdmin.CreateAsync(employee);
//            }
//        }

//        [Test]
//        // Assert
//        [ExpectedException(typeof(ValidationException))]
//        public async void Admin_CreateAsync_NotValidDaysSkipped_ValidationException()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = BuildMinValidEmployee();
//                employee.DaysSkipped = -1;

//                // Act
//                await _projectAssignmentServiceAdmin.CreateAsync(employee);
//            }
//        }

//        #endregion

//        #region Admin - Update

//        [Test]
//        public async void Admin_UpdateAsync_MinValidEmployee_Updated()
//        {
//            // Arrange
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//            using (TransactionScope scope = new TransactionScope())
//            {
//                Employee employee = BuildMinValidEmployee();
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);

//                Employee employeeToUpdate = BuildMinValidEmployee();
//                employeeToUpdate.EmployeeId = id.Value;

//                // Act
//                int updatedRowsCount = await _projectAssignmentServiceAdmin.UpdateAsync(employeeToUpdate);

//                Employee actualEmployee = await _projectAssignmentServiceAdmin.GetByIdAsync(id.Value);
//                //employee.EmployeeId = id.Value;
//                string expected = serializer.Serialize(employeeToUpdate);
//                string actual = serializer.Serialize(actualEmployee);

//                // Assert
//                Assert.AreEqual(1, updatedRowsCount);
//                Assert.AreEqual(expected, actual);
//            }
//        }

//        [Test]
//        public async void Admin_UpdateAsync_FilledValidEmployee_Updated()
//        {
//            // Arrange
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//            using (TransactionScope scope = new TransactionScope())
//            {
//                Employee employee = BuildFilledValidEmployee();
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);

//                Employee employeeToUpdate = BuildFilledValidEmployee();
//                employeeToUpdate.EmployeeId = id.Value;

//                // Act
//                int updatedRowsCount = await _projectAssignmentServiceAdmin.UpdateAsync(employee);


//                Employee actualEmployee = await _projectAssignmentServiceAdmin.GetByIdAsync(id.Value);
//                //employee.EmployeeId = id.Value;
//                string expected = serializer.Serialize(employeeToUpdate);
//                string actual = serializer.Serialize(actualEmployee);

//                // Assert
//                Assert.AreEqual(1, updatedRowsCount);
//                Assert.AreEqual(expected, actual);
//            }
//        }

//        [Test]
//        // Assert
//        [ExpectedException(typeof(ValidationException))]
//        public async void Admin_UpdateAsync_EmptyEmployee_ValidationException()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = BuildFilledValidEmployee();
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);

//                Employee employeeToUpdate = new Employee
//                {
//                    EmployeeId = id.Value
//                };

//                // Act
//                await _projectAssignmentServiceAdmin.UpdateAsync(employeeToUpdate);
//            }
//        }

//        [Test]
//        // Assert
//        [ExpectedException(typeof(ValidationException))]
//        public async void Admin_UpdateAsync_NotValidEmail_ValidationException()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = BuildMinValidEmployee();
//                employee.CompanyEmail = "notValidEmail";

//                // Act
//                await _projectAssignmentServiceAdmin.UpdateAsync(employee);
//            }
//        }

//        #endregion

//        #region Admin - Delete

//        [Test]
//        public async void Admin_DeleteAsync_ExistedEmployee_Deleted()
//        {
//            // Arrange
//            using (TransactionScope scope = new TransactionScope())
//            {
//                Employee employee = BuildMinValidEmployee();
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);
                
//                // Act
//                int deletedRowsCount = await _projectAssignmentServiceAdmin.DeleteAsync(id.Value);
                
//                Employee actualEmployee = await _projectAssignmentServiceAdmin.GetByIdAsync(id.Value);

//                // Assert
//                Assert.AreEqual(1, deletedRowsCount);
//                Assert.AreEqual(null, actualEmployee);
//            }
//        }

//        [Test]
//        public async void Admin_DeleteAsync_NotExistedEmployee_NotDeleted()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange

//                // Act
//                int deletedRowsCount = await _projectAssignmentServiceAdmin.DeleteAsync(Guid.NewGuid());
                
//                // Assert
//                Assert.AreEqual(0, deletedRowsCount);
//            }
//        }

//        [Test]
//        public async void Admin_DeleteAsync_DeletedEmployee_Deleted()
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Arrange
//                Employee employee = BuildFilledValidEmployee();
//                Guid? id = await _projectAssignmentServiceAdmin.CreateAsync(employee);

//                await _projectAssignmentServiceAdmin.DeleteAsync(id.Value);

//                // Act
//                int deletedRowsCount = await _projectAssignmentServiceAdmin.DeleteAsync(id.Value);

//                // Assert
//                Assert.AreEqual(1, deletedRowsCount);
//            }
//        }

//        #endregion

//        #region Builders

//        private static Employee BuildMinValidEmployee()
//        {
//            return new Employee
//            {
//                FullName = "FullName",
//                JobTitle = "Developer",
//                CompanyEmail = "email@gmail.com",
//                DepartmentName = "Development",
//                Department = new Department
//                {
//                    Name = "Development"
//                },
//                Project = new Project(),
//                OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Kharkiv"
//                },
//                StartDate = DateTime.UtcNow,
//                Status = (int)EmployeeStatus.Hired
//            };
//        }

//        private static Employee BuildFilledValidEmployee()
//        {
//            return new Employee
//            {
//                FullName = "FullName",
//                FullNameCyrillic = "Дорожко Ольга",
//                PatronymicCyrillic = "Олеговна",
//                JobTitle = ".NET developer",
//                Technology = ".NET",
//                ProjectName = "HR Tools",
//                DepartmentName = "Development",
//                Project = new Project
//                {
//                    Name = "HR Tools"
//                },
//                Department = new Department
//                {
//                    Name = "Development"
//                },
//                OfficeLocation = new OfficeLocation
//                {
//                    Country = "Ukraine",
//                    City = "Kharkiv"
//                },
//                CompanyEmail = "email@gmail.com",
//                PersonalEmail = "personalEmail@gmail.com",
//                Messenger = new Messenger
//                {
//                    Name = "skype",
//                    Login = "olha_dorozhko"
//                },
//                MobileNumber = "06312312312",
//                AdditionalMobileNumber = "06312312312",
//                Birthday = new DateTime(1992, 07, 20),
//                Status = (int)EmployeeStatus.Hired,
//                StartDate = DateTime.UtcNow,
//                TerminationDate = DateTime.UtcNow,
//                DaysSkipped = 0,
//                Notes = "It's me",
//                BioUrl = "http://someUrl",
//                PhotoUrl = "http://someUrl"
//            };
//        }

//        private static ProjectAssignmentGridSettings BuildDefaultGridSettings()
//        {
//            return new ProjectAssignmentGridSettings
//            {
//                PagingSettings = new PagingSettings
//                {
//                    CurrentPage = "1",
//                    ItemsPerPage = "5"
//                },
//                SortingSettings = new SortingSettings
//                {
//                    SortColumnName = "FullName",
//                    IsDescending = false
//                },
//                SearchKeyword = "",
//                ProjectAssignmentFilter = new ProjectAssignmentFilter()
//            };
//        }

//        #endregion
//    }
//}
