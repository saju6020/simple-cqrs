namespace Platform.Infrastructure.Core.Bus
{
    /// <summary>Bus queue message abastraction.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.Message" />
    /// <seealso cref="Platform.Infrastructure.Core.Bus.IBusQueueMessage" />
    public abstract class BusQueueMessage : Message, IBusQueueMessage
    {
        public string QueueName { get; set; }

        public void SetQueueName()
        {
            if (string.IsNullOrEmpty(this.QueueName))
            {
                this.QueueName = this.GetType().Namespace;
            }
        }
    }
}
