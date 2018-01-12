using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.Crosscutting.Common.Helpers;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Presentation.Admin.Api
{
    [Authorize(Roles = "Admin,SuperAdmin")]
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

        protected static Guid ClientId
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host;
                Guid clientId = CompanyHelper.GetCompanyIdByHost(host);

                return clientId;
            }
        }

        protected static string CompanyName
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host;
                string companyName = CompanyHelper.GetCompanyNameByHost(host);

                return companyName;
            }
        }

        private ILogger Logger => _logger.Value;
        
        protected async Task<HttpResponseMessage> TryExecuteAsync(Func<Task<HttpResponseMessage>> func)
        {
            try
            {
                Guard.ArgumentIsNotNull(func, nameof(func));

                return await func();
            }
            catch (ValidationException ex)
            {
                Logger.Error(ex);

                string message = ex.DisplayMessage ?? GeneralErrorMessage;
                
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, message);
            }
            catch (ConfigurationErrorsException ex)
            {
                Logger.Error(ex);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Website configuration is invalid. Please, contact your system administrator.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                // TODO: localize error and show special error page

                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, GeneralErrorMessage);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        protected HttpResponseMessage TryExecute(Func<HttpResponseMessage> func)
        {
            try
            {
                Guard.ArgumentIsNotNull(func, nameof(func));

               return func();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, GeneralErrorMessage);
            }
        }
    }
}
