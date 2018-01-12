using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Script.Serialization;
using HRTools.Core.Common.Enums;
using HRTools.Core.Common.Models;
using HRTools.Core.Common.Models.Employee;
using NLoad;
using NUnit.Framework;

namespace HRTools.Presentation.Tests.Admin.Core
{
    [TestFixture]
    public class EmployeeControllerTests: BasePerformanceTest
    {
        private const int TwoSeconds = 2000;
        private static readonly Uri SiteUrl = new Uri("http://teaminternational.admin:8086/");
        private static readonly HttpClient Client = new HttpClient();

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
                Status = (int) EmployeeStatus.Hired
            };
        }

        private class CreateEmployeeTest : Test
        {
            protected override async Task<TestResult> ExecuteAsync()
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Employee employee = BuildMinValidEmployee();

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    StringContent queryString = new StringContent(serializer.Serialize(employee));

                    await Client.PostAsync(@"/CreateEmployee", queryString);
                }

                return TestResult.Success;
            }
        }

        [Test]
        public void Admin_CreateEmployee()
        {
            LoadTestResult result = Test<CreateEmployeeTest>();

            Assert.Less(result.MaxResponseTime.Milliseconds, TwoSeconds);
            Console.WriteLine(result.TestRuns.Count);
        }

        [Test]
        public async void Admin_DeleteEmployee()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Employee employee = BuildMinValidEmployee();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StringContent queryString = new StringContent(serializer.Serialize(employee));
                await Client.PostAsync(@"/CreateEmployee", queryString);

                Stopwatch sw = new Stopwatch();

                sw.Start();

                await Client.PostAsync(@"/DeleteEmployee", new StringContent(Guid.Empty.ToString()));

                sw.Stop();

                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
            }
        }

        [Test]
        public async void Admin_GetAll()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            HttpResponseMessage response = await Client.GetAsync(
                @"/GetAll?gridSettings={TotalCount:0,CurrentPage:1,ItemsPerPage:2,SortColumnName:'FullName',IsDescending:false,SearchKeyword:'',EmployeeFilter:{}}");

            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
        }

        [Test]
        public async void Admin_GetCountriesWithCities()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            await Client.GetAsync(@"/GetCountriesWithCities");

            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
        }

        [Test]
        public async void Admin_UpdateEmployee()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Employee employee = BuildMinValidEmployee();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StringContent queryString = new StringContent(serializer.Serialize(employee));

                await Client.PostAsync(@"/CreateEmployee", queryString);

                Stopwatch sw = new Stopwatch();

                sw.Start();

                await Client.PostAsync(@"/UpdateEmployee", new StringContent(serializer.Serialize(employee)));

                sw.Stop();

                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
            }
        }
    }
}