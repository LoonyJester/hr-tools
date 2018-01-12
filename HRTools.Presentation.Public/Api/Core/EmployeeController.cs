using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using HRTools.Core.Common.Models;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Services.Company;
using HRTools.Core.Services.Employee;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Models;

namespace HRTools.Presentation.Public.Api.Core
{
    public class EmployeeController : BaseCoreApiController
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<ICompanyService> _companyService;

        public EmployeeController(
            Lazy<IEmployeeService> employeeService,
            Lazy<ICompanyService> companyService,
            Lazy<ILogger> logger) 
            : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(employeeService, nameof(employeeService));
            Guard.ConstructorArgumentIsNotNull(companyService, nameof(companyService));

            _employeeService = employeeService;
            _companyService = companyService;
        }

        private IEmployeeService EmployeeService => _employeeService.Value;
        private ICompanyService CompanyService => _companyService.Value;

        [HttpGet]
        [Route("Api/GetAllEmployees")]
        public Task<HttpResponseMessage> GetAllAsync(string gridSettings)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(gridSettings, nameof(gridSettings));

                EmployeeGridSettings settings = new JavaScriptSerializer().Deserialize<EmployeeGridSettings>(gridSettings);

                GrigData<Employee> employeeListGridData = await EmployeeService.GetAllAsync(settings);

                return Request.CreateResponse(HttpStatusCode.OK, employeeListGridData);
            });
        }
        
        [HttpGet]
        [Route("Api/GetCountriesWithCities")]
        public Task<HttpResponseMessage> GetCountriesWithCitiesAsync()
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<OfficeLocation> officeLocationList = await CompanyService.GetOfficeLocationListAsync();

                return Request.CreateResponse(HttpStatusCode.OK, officeLocationList.GroupBy(p => p.Country,
                    p => p.City,
                    (key, g) => new
                    {
                        Country = key,
                        Cities = g.ToList()
                    }));
            });
        }
    }
}