namespace Platform.Infrastructure.Core.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Dependencies;

    /// <summary>Class to publish events.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Events.IEventPublisher" />
    public class EventPublisher : IEventPublisher
    {
        private readonly IBusMessageDispatcher busMessageDispatcher;
        private readonly IHandlerResolver handlerResolver;
        private UserContext userContext;

        public EventPublisher(IBusMessageDispatcher busMessageDispatcher, UserContext userContext, IHandlerResolver handlerResolver)
        {
            this.busMessageDispatcher = busMessageDispatcher;
            this.handlerResolver = handlerResolver;
            this.userContext = userContext;
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            this.userContext = @event.UserContext;

            var handler = this.handlerResolver.ResolveHandler<IEventHandlerAsync<TEvent>>();
            await handler.HandleAsync(@event);

            if (@event is IBusTopicMessage message)
            {
                await this.busMessageDispatcher.DispatchAsync(message);
            }
        }

        public async Task PublishAsync<TEvent>(IEnumerable<TEvent> events)
            where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                await this.PublishAsync(@event);
            }
        }
    }
}
