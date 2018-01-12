using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.ProjectAssignment.Common.Models.Core;
using HRTools.ProjectAssignment.Services.Employee;

namespace HRTools.Presentation.Public.Api.ProjectAssignment
{
    // [Authorize(Roles = "Employee")]
    public class PAEmployeeController : BaseProjectAssignmentApiController
    {
        private readonly Lazy<IEmployeeService> _employeeService;

        public PAEmployeeController(
            Lazy<IEmployeeService> employeeService,
            Lazy<ILogger> logger)
            : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(employeeService, nameof(employeeService));

            _employeeService = employeeService;
        }

        private IEmployeeService EmployeeService => _employeeService.Value;

        [HttpGet]
        [Route("Api/GetEmployeesByNameAutocomplete")]
        public Task<HttpResponseMessage> GetAllAsync(string nameAutocomplete)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrWhiteSpace(nameAutocomplete, nameof(nameAutocomplete));

                IEnumerable<Employee> employeeList = await EmployeeService.GetAllByNameAutocompleteAsync(nameAutocomplete);

                return Request.CreateResponse(HttpStatusCode.OK, employeeList);
            });
        }
    }
}