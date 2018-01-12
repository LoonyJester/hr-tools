using System;
using System.Diagnostics;
using System.Net.Http;
using NUnit.Framework;

namespace HRTools.Presentation.Tests.Public.Core
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private const int TwoSeconds = 2000;
        private static readonly Uri SiteUrl = new Uri("http://teaminternational:8085/");
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

        [Test]
        public async void GetAll()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            await
                Client.GetAsync(
                    @"GetAll?gridSettings={PagingSettings:{CurrentPage:1,ItemsPerPage:2},SortingSettings:{SortColumnName: 'firstName'},SearchKeyword:'',EmployeeFilter:{}}");

            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
        }

        [Test]
        public async void GetCountriesWithCities()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            await Client.GetAsync(@"GetCountriesWithCities");

            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
        }
    }
}