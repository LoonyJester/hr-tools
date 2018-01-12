using System.Threading.Tasks;
using MassTransit.Audit;

namespace HRTools.Crosscutting.Messaging.Audit
{
    public interface IMessageAuditStoreService
    {
        Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class;
    }
}
