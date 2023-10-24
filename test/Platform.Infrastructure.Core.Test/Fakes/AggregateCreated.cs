namespace Core.UnitTest
{
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Domain;

    public class AggregateCreated : DomainEvent, IBusQueueMessage
    {
        public string QueueName { get; set; } = "queue-name";

        public void SetQueueName()
        {
            this.QueueName = "queue-name";
        }
    }
}
