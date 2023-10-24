namespace Platform.Infrastructure.Core.Bus
{
    public interface IBusTopicMessage : IMessage
    {
        string TopicName { get; set; }
    }
}
