using System.Threading.Tasks;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Messaging.Audit;
using MassTransit.Audit;

namespace HRTools.Presentation.Admin
{
    public class MessageAuditStore: IMessageAuditStore
    {
        private readonly IMessageAuditStoreService _messageAuditStoreService;

        public MessageAuditStore(IMessageAuditStoreService messageAuditStoreService)
        {
            Guard.ConstructorArgumentIsNotNull(messageAuditStoreService, nameof(messageAuditStoreService));

            _messageAuditStoreService = messageAuditStoreService;
        }

        public Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            return _messageAuditStoreService.StoreMessage(message, metadata);
        }
    }
}