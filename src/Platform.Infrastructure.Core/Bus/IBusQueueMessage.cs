namespace Platform.Infrastructure.Core.Bus
{
    public interface IBusQueueMessage : IMessage
    {
        string QueueName { get; set; }

        void SetQueueName();
    }
}
