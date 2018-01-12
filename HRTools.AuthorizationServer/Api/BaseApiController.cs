using System;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.AuthorizationServer.Api
{
    [Authorize]
    public abstract class BaseApiController : ApiController
    {
        private readonly Lazy<ILogger> _logger;

        private const string GeneralErrorMessage = "Some error has occured";

        public BaseApiController(Lazy<ILogger> logger)
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
            catch (ValidationException ex)
            {
                Logger.Error(ex);

                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message);
            }
            catch (ConfigurationErrorsException ex)
            {
                Logger.Error(ex);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Website configuration is invalid. Please, contact your system administrator.");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, GeneralErrorMessage);
            }
        }
    }
}