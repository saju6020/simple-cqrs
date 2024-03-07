namespace Platform.Infrastructure.Core.Bus
{
    using System;
    using System.Threading.Tasks;    

    /// <summary>Bus message dispatcher. This class will provide methods to dispatch message.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.IBusMessageDispatcher" />
    public class BusMessageDispatcher : IBusMessageDispatcher
    {
        private readonly IBusProvider busProvider;

        public BusMessageDispatcher(IBusProvider busProvider)
        {
            this.busProvider = busProvider;
        }

        /// <summary>Dispatches the asynchronous.</summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>Return void task.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The message cannot implement both the IBusQueueMessage and the IBusTopicMessage interfaces
        /// or
        /// The message must implement either the IBusQueueMessage or the IBusTopicMessage interface.
        /// </exception>
        public Task DispatchAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            if (message is IBusQueueMessage && message is IBusTopicMessage)
            {
                throw new NotSupportedException("The message cannot implement both the IBusQueueMessage and the IBusTopicMessage interfaces");
            }

            if (message is IBusQueueMessage queueMessage)
            {
                return this.busProvider.SendQueueMessageAsync(queueMessage);
            }

            if (message is IBusTopicMessage topicMessage)
            {
                return this.busProvider.SendTopicMessageAsync(topicMessage);
            }

            throw new NotSupportedException("The message must implement either the IBusQueueMessage or the IBusTopicMessage interface");
        }

        public Task PublishAsync<TMessage>(TMessage @event)
           where TMessage : IBusTopicMessage
        {
            return this.busProvider.SendTopicMessageAsync((IBusTopicMessage)@event);
        }

        public Task SendAsync<TMessage>(TMessage command)
            where TMessage : IBusQueueMessage
        {
            if (command.CorrelationId.Equals(Guid.Empty))
            {
                throw new Exception("IBusQueueMessage or command must have CorrelationId");
            }

            return this.busProvider.SendQueueMessageAsync((IBusQueueMessage)command);
        }
    }
}
