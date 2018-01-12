//using System;
//using System.Diagnostics;
//using System.Net.Http;
//using System.Transactions;
//using System.Web.Script.Serialization;
//using HRTools.ProjectAssignment.Common.Models.Project;
//using NUnit.Framework;

//namespace HRTools.Presentation.Tests.Public.ProjectAssignment
//{
//    [TestFixture]
//    public class ProjectAssignmentControllerTests
//    {
//        private const int TwoSeconds = 2000;
//        private static readonly Uri SiteUrl = new Uri("http://teaminternational:8085/");
//        readonly static HttpClient Client = new HttpClient();

//        public ProjectAssignmentControllerTests()
//        {
//            if (Client.BaseAddress == null)
//            {
//                Client.BaseAddress = SiteUrl;
//                Client.DefaultRequestHeaders.Add("Authorization", "Bearer a3e767294934dd921bdaeeb9af9c8277");
//            }
//        }

//        [SetUpFixture]
//        public class MySetUpClass
//        {
//            [SetUp]
//            public void RunBeforeAnyTests()
//            {
//                Client.BaseAddress = SiteUrl;
//                Client.DefaultRequestHeaders.Add("Authorization", "Bearer a3e767294934dd921bdaeeb9af9c8277");
//            }
//        }

//        [Test]
//        public async void Admin_GetAllProjectAssignments()
//        {
//            Stopwatch sw = new Stopwatch();

//            sw.Start();

//            await Client.GetAsync(@"/GetAllProjectAssignments?gridSettings={TotalCount:0,CurrentPage:1,ItemsPerPage:2,SortColumnName:'FullName',IsDescending:false,SearchKeyword:'',ProjectAssignmentFilter:{}}");

//            sw.Stop();

//            Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

//            Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
//        }

//        [Test]
//        public async void Admin_CreateProjectAssignment()
//        {
//            using (var scope = new TransactionScope())
//            {
//                ProjectAsi project = new Project
//                {
//                    Id = 1,
//                    Name = "QA",
//                    Description = "QA department",
//                    StartDate = DateTime.Now.AddMonths(-3)
//                };

//                JavaScriptSerializer serializer = new JavaScriptSerializer();
//                StringContent queryString = new StringContent(serializer.Serialize(project));

//                Stopwatch sw = new Stopwatch();

//                sw.Start();

//                await Client.PostAsync(@"/CreateProject", queryString);

//                sw.Stop();

//                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

//                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
//            }
//        }

//        [Test]
//        public async void Admin_UpdateProject()
//        {
//            using (var scope = new TransactionScope())
//            {
//                Project project = new Project
//                {
//                    Id = 1,
//                    Name = "QA",
//                    Description = "QA department",
//                    StartDate = DateTime.Now.AddMonths(-3)
//                };

//                JavaScriptSerializer serializer = new JavaScriptSerializer();
//                StringContent queryString = new StringContent(serializer.Serialize(project));

//                await Client.PostAsync(@"/CreateProject", queryString);

//                Stopwatch sw = new Stopwatch();

//                sw.Start();

//                await Client.PostAsync(@"/UpdateProject", new StringContent(serializer.Serialize(project)));

//                sw.Stop();

//                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

//                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
//            }
//        }

//        [Test]
//        public async void Admin_DeleteProject()
//        {
//            using (var scope = new TransactionScope())
//            {
//                Project project = new Project
//                {
//                    Id = 1,
//                    Name = "QA",
//                    Description = "QA department",
//                    StartDate = DateTime.Now.AddMonths(-3)
//                };

//                JavaScriptSerializer serializer = new JavaScriptSerializer();
//                StringContent queryString = new StringContent(serializer.Serialize(project));
//                await Client.PostAsync(@"/CreateProject", queryString);

//                Stopwatch sw = new Stopwatch();

//                sw.Start();

//                await Client.PostAsync(@"/DeleteProject", new StringContent(project.Id.ToString()));

//                sw.Stop();

//                Console.WriteLine($"{sw.ElapsedMilliseconds} miliseconds");

//                Assert.Less(sw.ElapsedMilliseconds, TwoSeconds);
//            }
//        }
//    }
//}
