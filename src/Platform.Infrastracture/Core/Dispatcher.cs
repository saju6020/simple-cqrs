namespace Platform.Infrastructure.Core
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Queries;

    /// <summary>Dispatcher class to dispatch message.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.IDispatcher" />
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandSender commandSender;
        private readonly IEventPublisher eventPublisher;
        private readonly IQueryProcessor queryProcessor;
        private readonly IBusMessageDispatcher busMessageDispatcher;
        private readonly IUserContextProvider userContextProvider;

        public Dispatcher(
            ICommandSender commandSender,
            IEventPublisher eventPublisher,
            IQueryProcessor queryProcessor,
            IBusMessageDispatcher busMessageDispatcher,
            IUserContextProvider userContextProvider)
        {
            this.commandSender = commandSender;
            this.eventPublisher = eventPublisher;
            this.queryProcessor = queryProcessor;
            this.busMessageDispatcher = busMessageDispatcher;
            this.userContextProvider = userContextProvider;
        }

        public Task SendBusMessageAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            this.SetUserContextWithMessage(message);
            return this.busMessageDispatcher.DispatchAsync(message);
        }

        public Task PublishBusMessageAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            this.SetUserContextWithMessage(message);
            return this.busMessageDispatcher.DispatchAsync(message);
        }

        public async Task<CommandResponse> SendAsync(ICommand command)
        {
            CommandResponse response = new CommandResponse();

            this.SetUserContextWithMessage(command);

            if (command.IsInMemoryCommand)
            {
                response = await this.commandSender.SendAsync(command);
            }
            else
            {
                await this.busMessageDispatcher.DispatchAsync(command);
            }

            return response;
        }

        public Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            this.SetUserContextWithMessage(@event);
            return this.eventPublisher.PublishAsync(@event);
        }

        public async Task<QueryResponse<TResult>> GetResultAsync<TResult>(IQuery<TResult> query)
        {
            this.SetUserContextWithMessage(query);
            return await this.queryProcessor.ProcessAsync(query).ConfigureAwait(false);
        }

        public async Task<QueryResponse<TResult>> GetResultAsync<TResult>()
        {
            return await this.queryProcessor.ProcessAsync<TResult>().ConfigureAwait(false);
        }

        //public CommandResponse Send(ICommand command)
        //{
        //    this.SetUserContextWithMessage(command);
        //    return this.commandSender.Send(command);
        //}

        //public CommandResponse Send<TResult>(ICommand command)
        //{
        //    this.SetUserContextWithMessage(command);
        //    return this.commandSender.Send(command);
        //}

        public QueryResponse<TResult> GetResult<TResult>(IQuery<TResult> query)
        {
            this.SetUserContextWithMessage(query);
            return this.queryProcessor.Process(query);
        }

        public QueryResponse<TResult> GetResult<TResult>()
        {
            return this.queryProcessor.Process<TResult>();
        }

        /// <summary>Sets the user context with message.</summary>
        /// <param name="message">The message.</param>
        private void SetUserContextWithMessage(ISecurityInfo message)
        {
            message.SetUserContext(this.userContextProvider.GetUserContext());
        }
    }
}