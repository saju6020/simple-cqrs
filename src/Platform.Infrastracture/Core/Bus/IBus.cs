namespace Platform.Infrastructure.Core.Bus
{
    using System.Threading.Tasks;

    public interface IBus
    {
        Task SendAsync<T>(T command)
            where T : IBusQueueMessage;

        Task PublishAsync<T>(T @event)
            where T : IBusTopicMessage;
    }
}
