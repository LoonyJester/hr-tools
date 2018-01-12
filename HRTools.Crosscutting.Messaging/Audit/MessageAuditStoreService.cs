using System.Threading.Tasks;
using HRTools.Crosscutting.Common;
using MassTransit.Audit;

namespace HRTools.Crosscutting.Messaging.Audit
{
    public class MessageAuditStoreService: IMessageAuditStoreService
    {
        private readonly IMessageAuditStoreRepository _messageAuditStoreRepository;

        public MessageAuditStoreService(IMessageAuditStoreRepository messageAuditStoreRepository)
        {
            Guard.ConstructorArgumentIsNotNull(messageAuditStoreRepository, nameof(messageAuditStoreRepository));

            _messageAuditStoreRepository = messageAuditStoreRepository;
        }

        public Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            return _messageAuditStoreRepository.StoreMessage(message, metadata);
        }
    }
}
