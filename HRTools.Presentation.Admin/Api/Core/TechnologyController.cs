using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Core.Common.Models;
using HRTools.Core.Services.Technology;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Presentation.Admin.Api.Core
{
    public class TechnologyController : BaseCoreApiController
    {
        private readonly Lazy<ITechnologyServiceAdmin> _tehcnologyService;

        public TechnologyController(
            Lazy<ITechnologyServiceAdmin> tehcnologyService,
            Lazy<ILogger> logger) : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(tehcnologyService, nameof(tehcnologyService));

            _tehcnologyService = tehcnologyService;
        }

        private ITechnologyServiceAdmin TehcnologyService => _tehcnologyService.Value;

        [HttpGet]
        [Route("Api/GetAllTechnologies")]
        public Task<HttpResponseMessage> GetAllAsync()
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<Technology> jobTitleList = await TehcnologyService.GetAllAsync();

                return Request.CreateResponse(HttpStatusCode.OK, jobTitleList);
            });
        }

        [HttpGet]
        [Route("Api/Test")]
        public object Test()
        {
            ClaimsPrincipal user = User as ClaimsPrincipal;

            return user.Claims.Select(x => new
            {
                Type = x.Type,
                ValueType = x.Value
            });
        }

        [HttpPost]
        [Route("Api/CreateTechnology")]
        public Task<HttpResponseMessage> CreateAsync([FromBody] Technology technology)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(technology, nameof(technology));

                int id = await TehcnologyService.CreateAsync(technology);

                return id == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Technology was not successfully created")
                    : Request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

        [HttpPut]
        [Route("Api/UpdateTechnology")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] Technology technology)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(technology, nameof(technology));

                int updatedRowsCount = await TehcnologyService.UpdateAsync(technology);

                return updatedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Technology was not successfully updated")
                    : Request.CreateResponse(HttpStatusCode.OK, updatedRowsCount);
            });
        }

        [HttpDelete]
        [Route("Api/DeleteTechnology")]
        public Task<HttpResponseMessage> DeleteAsync([FromBody] int id)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(id, nameof(id));

                int deletedRowsCount = await TehcnologyService.DeleteAsync(id);

                return deletedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Technology was not successfully deleted")
                    : Request.CreateResponse(HttpStatusCode.OK, deletedRowsCount);
            });
        }
    }
}