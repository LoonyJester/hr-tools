using System.Threading.Tasks;

namespace HRTools.Crosscutting.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T eventToPublish) where T : IBaseEvent;
    }
}
