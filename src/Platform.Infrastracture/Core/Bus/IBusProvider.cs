namespace Platform.Infrastructure.Core.Bus
{
    using System;
    using System.Threading.Tasks;

    public interface IBusProvider
    {
        
        Task SendQueueMessageAsync<TMessage>(TMessage message)
            where TMessage : IBusQueueMessage;

     
        Task SendTopicMessageAsync<TMessage>(TMessage message)
            where TMessage : IBusTopicMessage;
    }
}
