namespace Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Bus;

    public class BusProvider : IBusProvider
    {
        private readonly IBusControl busControl;

        public BusProvider(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public async Task SendQueueMessageAsync<TMessage>(TMessage message)
            where TMessage : IBusQueueMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.CorrelationId.Equals(Guid.Empty))
            {
                throw new InvalidOperationException("IBusQueueMessage must have CorrelationId");
            }

            var queueName = string.IsNullOrWhiteSpace(message.QueueName) == false ? message.QueueName : message.GetType().Namespace;

            var sendToUri = new Uri($"queue:{queueName}");

            var endPoint = await this.busControl.GetSendEndpoint(sendToUri);

            await endPoint.Send(message);
        }

        public Task SendTopicMessageAsync<TMessage>(TMessage message)
            where TMessage : IBusTopicMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return this.busControl.Publish(message);
        }
    }
}
