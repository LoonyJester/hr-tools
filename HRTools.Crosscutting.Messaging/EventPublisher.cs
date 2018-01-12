using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Helpers;
using MassTransit;

namespace HRTools.Crosscutting.Messaging
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            Guard.ConstructorArgumentIsNotNull(publishEndpoint, nameof(publishEndpoint));

            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T eventToPublish) where T : IBaseEvent
        {
            Guard.ArgumentIsNotNull(eventToPublish, nameof(eventToPublish));

            string rabbitMqHostUrl = WebConfigurationManager.AppSettings["RabbitMqHostUrl"];
            string host = HttpContext.Current.Request.Url.Host;
            string companyName = CompanyHelper.GetCompanyNameByHost(host);

            if (string.IsNullOrEmpty(rabbitMqHostUrl) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(companyName))
            {
                throw new ConfigurationErrorsException("Configuration is invalid");
            }
            
            return _publishEndpoint.Publish(eventToPublish, context =>
            {
                context.FaultAddress = new Uri($"{rabbitMqHostUrl}/{companyName}_errors");
                context.Durable = true;
                eventToPublish.EventId = context.MessageId ?? Guid.NewGuid();
            });
        }
    }
}
