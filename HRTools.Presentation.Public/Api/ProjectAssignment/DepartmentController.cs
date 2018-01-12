using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.ProjectAssignment.Common.Models.Department;
using HRTools.ProjectAssignment.Services.Department;

namespace HRTools.Presentation.Public.Api.ProjectAssignment
{
    public class DepartmentController : BaseProjectAssignmentApiController
    {
        private readonly Lazy<IDepartmentService> _departmentService;

        public DepartmentController(
            Lazy<IDepartmentService> departmentService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(departmentService, nameof(departmentService));

            _departmentService = departmentService;
        }

        private IDepartmentService DepartmentService => _departmentService.Value;

        [HttpGet]
        [Route("Api/GetAllDepartments")]
        public Task<HttpResponseMessage> GetAllAsync()
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<Department> userListTableData = await DepartmentService.GetAllAsync();

                return Request.CreateResponse(HttpStatusCode.OK, userListTableData);
            });
        }
    }
}