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

namespace HRTools.Presentation.Admin.Api.ProjectAssignment
{
    public class DepartmentController : BaseProjectAssignmentApiController
    {
        private readonly Lazy<IDepartmentServiceAdmin> _projectService;

        public DepartmentController(
            Lazy<IDepartmentServiceAdmin> projectService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(projectService, nameof(projectService));

            _projectService = projectService;
        }

        private IDepartmentServiceAdmin DepartmentService => _projectService.Value;

        [HttpGet]
        [Route("Api/GetAllDepartments")]
        public Task<HttpResponseMessage> GetAllAsync(string searchKeyword)
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<Department> userListTableData = await DepartmentService.GetAllAsync(searchKeyword);

                return Request.CreateResponse(HttpStatusCode.OK, userListTableData);
            });
        }

        [HttpPost]
        [Route("Api/CreateDepartment")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] Department department)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(department, nameof(department));

                int id = await DepartmentService.CreateAsync(department);

                return id == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Department was not successfully created")
                    : Request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

        [HttpPut]
        [Route("Api/UpdateDepartment")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] Department department)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(department, nameof(department));

                int updatedRowsCount = await DepartmentService.UpdateAsync(department);

                return updatedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Department was not successfully updated")
                    : Request.CreateResponse(HttpStatusCode.OK, updatedRowsCount);
            });
        }

        [HttpDelete]
        [Route("Api/DeleteDepartment")]
        public Task<HttpResponseMessage> DeleteAsync([FromBody] int id)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(id, nameof(id));

                int deletedRowsCount = await DepartmentService.DeleteAsync(id);

                return deletedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Department was not successfully deleted")
                    : Request.CreateResponse(HttpStatusCode.OK, deletedRowsCount);
            });
        }
    }
}