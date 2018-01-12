
using System;
using System.Web.Configuration;
using System.Web.Hosting;
using GreenPipes;
using HRTools.Crosscutting.Common.Helpers;
using HRTools.Crosscutting.Messaging.Events.Employee;
using MassTransit;
using MassTransit.Audit;
using MassTransit.NLogIntegration;
using SimpleInjector;

namespace HRTools.Presentation.Admin.App_Start
{
    public static class MessageBusConfig
    {
        public static void Configure(Container container, out IBusControl busControl)
        {
            string rabbitMqHostUrl = WebConfigurationManager.AppSettings["RabbitMqHostUrl"];
            string rabbitMqHostLogin = WebConfigurationManager.AppSettings["RabbitMqHostLogin"];
            string rabbitMqHostPassword = WebConfigurationManager.AppSettings["RabbitMqHostPassword"];

            busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                string queueName = GetQueueName();

                var host = cfg.Host(new Uri(rabbitMqHostUrl), h =>
                {
                    h.Username(rabbitMqHostLogin);
                    h.Password(rabbitMqHostPassword);
                });

                cfg.UseRetry(x => x.Interval(10, TimeSpan.FromMilliseconds(500)));

                cfg.ReceiveEndpoint(host, queueName, e =>
                {
                    e.Consumer(typeof(IConsumer<IEmployeeCreatedEvent>), type => container.GetInstance(type));
                    e.Consumer(typeof(IConsumer<IEmployeeUpdatedEvent>), type => container.GetInstance(type));
                    e.Consumer(typeof(IConsumer<IEmployeeDeletedEvent>), type => container.GetInstance(type));

                    e.Durable = true;

                    //e.UseCircuitBreaker(cb =>
                    //{
                    //    cb.TrackingPeriod = TimeSpan.FromMinutes(2);
                    //    cb.TripThreshold = 15;
                    //    cb.ActiveThreshold = 10;
                    //    cb.ResetInterval = TimeSpan.FromMinutes(2);
                    //});
                });

                cfg.UseNLog();
            });
        }

        private static string GetQueueName()
        {
            string siteName = HostingEnvironment.ApplicationHost.GetSiteName();

            return CompanyHelper.GetCompanyNameByHost(siteName);
        }

        public static void ConfigureWhenContainerIsVerified(Container container, IBusControl busControl)
        {
            IMessageAuditStore store = container.GetInstance(typeof(MessageAuditStore)) as IMessageAuditStore;
            busControl.ConnectConsumeAuditObserver(store);

            busControl.Start();
        }
    }
}