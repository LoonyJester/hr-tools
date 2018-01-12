using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Presentation.Public.Api
{
    [Authorize(Roles = "Employee")]
    public abstract class BaseApiController : ApiController
    {
        protected const int SecondInMiliseconds = 1000;

        private const string GeneralErrorMessage = "Some error has occured";

        private readonly Lazy<ILogger> _logger;

        protected BaseApiController(Lazy<ILogger> logger)
        {
            Guard.ConstructorArgumentIsNotNull(logger, nameof(logger));

            _logger = logger;
        }

        private ILogger Logger => _logger.Value;
        
        protected async Task<HttpResponseMessage> TryExecuteAsync(Func<Task<HttpResponseMessage>> func)
        {
            try
            {
                Guard.ArgumentIsNotNull(func, nameof(func));

                return await func();
            }
            catch (ConfigurationErrorsException ex)
            {
                Logger.Error(ex);
                
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Website configuration is invalid. Please, contact your system administrator.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

               // return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, GeneralErrorMessage);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
