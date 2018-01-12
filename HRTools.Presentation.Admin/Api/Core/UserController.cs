using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Services.Employee;
using HRTools.Core.Services.User;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Models;
using HRTools.Crosscutting.Common.Models.User;

namespace HRTools.Presentation.Admin.Api.Core
{
    public class UserController : BaseCoreApiController
    {
        private readonly Lazy<IEmployeeServiceAdmin> _employeeService;
        private readonly Lazy<IUserServiceAdmin> _userService;

        public UserController(
            Lazy<IUserServiceAdmin> userService,
            Lazy<IEmployeeServiceAdmin> employeeService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(userService, nameof(userService));
            Guard.ConstructorArgumentIsNotNull(employeeService, nameof(employeeService));

            _userService = userService;
            _employeeService = employeeService;
        }

        private IUserServiceAdmin UserService => _userService.Value;
        private IEmployeeServiceAdmin EmployeeService => _employeeService.Value;
        
        //[HttpGet]
        //[Route("Api/GetAllUsers")]
        //public Task<HttpResponseMessage> GetAllBySiteId(string tableSettings)
        //{
        //    return TryExecuteAsync(async () =>
        //    {
        //        UserTableSettings settings = new JavaScriptSerializer().Deserialize<UserTableSettings>(tableSettings);
        //        Guard.ArgumentIsNotNull(settings, nameof(settings));

        //        GrigData<User> userListTableData = await UserService.GetAllAsync(settings);

        //        return Request.CreateResponse(HttpStatusCode.OK, userListTableData);
        //    });
        //}

        [HttpPost]
        [Route("Api/AssignUserToEmployee")]
        public Task<HttpResponseMessage> AssignUserToEmployee([FromBody] dynamic model)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(model, nameof(model));
                Guard.ArgumentIsNotNullOrEmpty(model.UserId.ToString(), nameof(model.UserId));
                Guard.ArgumentIsNotNullOrEmpty(model.Login.ToString(), nameof(model.Login));

                string userId = model.UserId.ToString();
                string login = model.Login.ToString();

                Employee employeeToAssign = await EmployeeService.GetByCompanyEmailAsync(login);

                if (employeeToAssign == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Employee was not assigned to user: there is no employee with same CompanyEmail");
                }

                int wasAssigned = await UserService.AssignUserToEmployeeAsync(userId, login);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    wasAssigned,
                    fullName = employeeToAssign.FullName
                });
            });
        }
    }
}