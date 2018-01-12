using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Web.Script.Serialization;
using HRTools.Core.Common.Enums;
using HRTools.Core.Common.Models;
using HRTools.Core.Common.Models.Employee;
using HRTools.Crosscutting.Common.Models.User;
using NUnit.Framework;

namespace HRTools.Presentation.Tests.Admin.Core
{
    [TestFixture]
    public class UserControllerTests
    {
        private const int TwoSeconds = 2000;
        private static readonly Uri SiteUrl = new Uri("http://teaminternational.admin:8086/");
        readonly static HttpClient Client = new HttpClient();

        [SetUpFixture]
        public class MySetUpClass
        {
            [SetUp]
            public void RunBeforeAnyTests()
            {
                Client.BaseAddress = SiteUrl;
                Client.DefaultRequestHeaders.Add("Authorization", "Bearer a3e767294934dd921bdaeeb9af9c8277");
            }
        }

        [Test]
        public async void Admin_GetAll()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            
            await Client.GetAsync(@"/GetAllUsers?tableSettings={PagingSettings:{CurrentPage:1,ItemsPerPage:2},SearchKeyword:'',EmployeeFilter:{}}");
            
            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
        }

        [Test]
        [Ignore]
        // TODO: load certificate and make call to Auth
        public async void Admin_AssignUserToEmployee()
        {
            using (var scope = new TransactionScope())
            {
                User user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    FullName = "User name",
                    IsActivated = true,
                    Login = "email@mail.com",
                    Password = "Welcome123!",
                    Roles = new List<Role>
                    {
                        new Role
                        {
                            Id = "1",
                            Name = "Employee"
                        }
                    } 
                };

                WebRequestHandler handler = new WebRequestHandler();
                X509Certificate2 certificate = new X509Certificate2();// GetMyX509Certificate();
                handler.ClientCertificates.Add(certificate);
                HttpClient client = new HttpClient(handler);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StringContent queryString = new StringContent(serializer.Serialize(user));

                // create User in Auth service
                await client.PostAsync(@"https://teaminternational.auth:44311/CreateUser", queryString);

                // create employee with same companyEmail

                Employee employee = BuildMinValidEmployee();
                employee.CompanyEmail = "email@mail.com";

                await Client.PostAsync(@"/CreateEmployee", queryString);

                Stopwatch sw = new Stopwatch();

                sw.Start();

                await Client.PostAsync(@"/AssignUserToEmployee", new StringContent(serializer.Serialize(new
                {
                    UserId = user.UserId,
                    Login = user.Login
                })));

                sw.Stop();

                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
            }
        }

        //public static X509Certificate2 Load()
        //{
        //    var assembly = typeof(Certificate).Assembly;

        //    //string[] a = assembly.GetManifestResourceNames();

        //    using (var stream = assembly.GetManifestResourceStream("HRTools.AuthorizationServer.Configuration.idsrv3test.pfx"))
        //    {
        //        return new X509Certificate2(ReadStream(stream), "idsrv3test");
        //    }
        //}

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private static Employee BuildMinValidEmployee()
        {
            return new Employee
            {
                FullName = "FullName",
                JobTitle = "Developer",
                CompanyEmail = "email@gmail.com",
                DepartmentName = "Development",
                OfficeLocation = new OfficeLocation
                {
                    Country = "Ukraine",
                    City = "Kharkiv"
                },
                StartDate = DateTime.UtcNow,
                Status = (int)EmployeeStatus.Hired
            };
        }
    }
}
