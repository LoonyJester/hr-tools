using System;
using System.Diagnostics;
using System.Net.Http;
using System.Transactions;
using System.Web.Script.Serialization;
using HRTools.ProjectAssignment.Common.Models.Department;
using NUnit.Framework;

namespace HRTools.Presentation.Tests.Admin.ProjectAssignment
{
    [TestFixture]
    public class DepartmentControllerTests
    {
        private const int TwoSeconds = 2000;
        private static readonly Uri SiteUrl = new Uri("http://teaminternational.admin:8086/");
        readonly static HttpClient Client = new HttpClient();

        [SetUpFixture]
        public class SetUpFixture
        {
            [SetUp]
            public void RunBeforeAnyTests()
            {
                Client.BaseAddress = SiteUrl;
                Client.DefaultRequestHeaders.Add("Authorization", "Bearer a3e767294934dd921bdaeeb9af9c8277");
            }
        }

        [Test]
        public async void Admin_GetAllDepartments()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            
            await Client.GetAsync(@"/GetAllDepartments?searchKeyword=''");
            
            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
        }

        [Test]
        public async void Admin_CreateDepartment()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Department department = new Department
                {
                    Id = 1,
                    Name = "QA",
                    Description = "QA department"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StringContent queryString = new StringContent(serializer.Serialize(department));

                Stopwatch sw = new Stopwatch();

                sw.Start();

                await Client.PostAsync(@"/CreateDepartment", queryString);

                sw.Stop();

                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
            }
        }

        [Test]
        public async void Admin_UpdateDepartment()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Department department = new Department
                {
                    Id = 1,
                    Name = "QA",
                    Description = "QA department"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StringContent queryString = new StringContent(serializer.Serialize(department));

                await Client.PostAsync(@"/CreateDepartment", queryString);

                Stopwatch sw = new Stopwatch();

                sw.Start();

                await Client.PostAsync(@"/UpdateDepartment", new StringContent(serializer.Serialize(department)));

                sw.Stop();

                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
            }
        }

        [Test]
        public async void Admin_DeleteDepartment()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Department department = new Department
                {
                    Id = 1,
                    Name = "QA",
                    Description = "QA department"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StringContent queryString = new StringContent(serializer.Serialize(department));
                await Client.PostAsync(@"/CreateDepartment", queryString);

                Stopwatch sw = new Stopwatch();

                sw.Start();

                await Client.PostAsync(@"/DeleteDepartment", new StringContent(department.Id.ToString()));

                sw.Stop();

                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
            }
        }
    }
}
