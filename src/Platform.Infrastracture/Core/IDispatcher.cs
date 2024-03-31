namespace Platform.Infrastructure.Core
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Queries;

    /// <summary>Dispatcher Interface.</summary>
    public interface IDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;

        Task<QueryResponse<TResult>> GetResultAsync<TResult>(IQuery<TResult> query);

        Task<QueryResponse<TResult>> GetResultAsync<TResult>();

        QueryResponse<TResult> GetResult<TResult>(IQuery<TResult> query);

        QueryResponse<TResult> GetResult<TResult>();

       // CommandResponse Send(ICommand command);

        Task<CommandResponse> SendAsync(ICommand command);

        Task SendBusMessageAsync<TMessage>(TMessage message)
            where TMessage : IMessage;

        Task PublishBusMessageAsync<TMessage>(TMessage message)
            where TMessage : IMessage;
    }
}
