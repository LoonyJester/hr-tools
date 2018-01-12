using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using HRTools.AuthorizationServer.Services;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Models;
using HRTools.Crosscutting.Common.Models.User;
using Microsoft.AspNet.Identity;
using CoreModels = HRTools.Crosscutting.Common.Models.User;

namespace HRTools.AuthorizationServer.Api
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseApiController
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService, Lazy<ILogger> logger) : base(logger)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("Api/GetAllRoles")]
        public Task<HttpResponseMessage> GetAllRoles()
        {
#pragma warning disable 1998
            return TryExecuteAsync(async () =>
#pragma warning restore 1998
            {
                List<Role> roles = new List<Role>();
                try
                {
                    roles = await _authService.GetAllRolesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }


                return Request.CreateResponse(HttpStatusCode.OK, roles);
            });
        }
        
        [HttpGet]
        [Route("Api/GetAllUsers")]
        public  Task<HttpResponseMessage> GetAll(string tableSettings)
        {
            return TryExecuteAsync(async () =>
            {
                UserTableSettings settings = new JavaScriptSerializer().Deserialize<UserTableSettings>(tableSettings);
                Guard.ArgumentIsNotNull(settings, nameof(settings));

                GrigData<CoreModels.User> userListTableData = await _authService.GetAllAsync(settings);

                return Request.CreateResponse(HttpStatusCode.OK, userListTableData);
            });
        }

        [HttpPost]
        [Route("Api/CreateUser")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] CoreModels.User user)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(user, nameof(user));

                string userId = await _authService.CreateUserAsync(user);

                return Request.CreateResponse(HttpStatusCode.OK, userId);
            });
        }

        [HttpPost]
        [Route("Api/UpdateUser")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] CoreModels.User user)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(user, nameof(user));

                IdentityResult result = await _authService.UpdateUserAsync(user);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [HttpPost]
        [Route("Api/ActivateUser")]
        public Task<HttpResponseMessage> ActivateUser([FromBody] dynamic model)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(model, nameof(model));
                Guard.ArgumentIsNotNullOrEmpty(model.UserId.ToString(), nameof(model.UserId));
                Guard.ArgumentIsNotNull(model.Activate, nameof(model.Activate));

                IdentityResult result = await _authService.ActivateUserAsync(model.UserId.ToString(), (bool) model.Activate);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }
    }
}
