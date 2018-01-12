using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using MassTransit.Audit;
using Newtonsoft.Json;

namespace HRTools.Crosscutting.Messaging.Audit
{
    public class MessageAuditStoreRepository : Repository, IMessageAuditStoreRepository
    {
        public MessageAuditStoreRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }

        public Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            Guard.ArgumentIsNotNull(message, nameof(message));
            Guard.ArgumentIsNotNull(metadata, nameof(metadata));

            string serializedMessage = JsonConvert.SerializeObject(message);
            string serializedMetadata = JsonConvert.SerializeObject(metadata);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Message", serializedMessage);
            parameters.Add("Metadata", serializedMetadata);
            parameters.Add("DateTime", DateTime.UtcNow);
            parameters.Add("ThreadId", Thread.CurrentThread.ManagedThreadId);

            return ExecuteAsync("messagesaudit_create", parameters);
        }
    }
}