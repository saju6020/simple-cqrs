namespace Core.UnitTest.Fakes
{
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Events;

    public class SomethingCreated : Event, IBusQueueMessage
    {
        public string QueueName { get; set; } = "queue-name";

        public void SetQueueName()
        {
            this.QueueName = "queue-name";
        }
    }
}
