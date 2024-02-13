namespace Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using MassTransit.Context;
    using MassTransit.Saga;
    using Microsoft.Extensions.Logging;
    using Platform.Infrastructure.Core.Accessors;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Models;
    using Event = Core.Events.Event;

    public class MassTransitGenericMessageFilter<T> : IFilter<ConsumeContext<T>>
        where T : Message
    {
        private const string HandlerMethodName = "HandleAsync";

        private readonly IContextAccessor contextAccessor;
        private readonly IServiceProvider serviceProvider;
        private readonly ICorrelationIdAccessor correlationIdAccessor;
        private readonly ILogger<MassTransitGenericMessageFilter<T>> logger;

        public MassTransitGenericMessageFilter(
            IServiceProvider serviceProvider,
            IContextAccessor contextAccessor,
            ICorrelationIdAccessor correlationIdAccessor,
            ILogger<MassTransitGenericMessageFilter<T>> logger)
        {
            this.logger = logger;
            this.contextAccessor = contextAccessor;
            this.correlationIdAccessor = correlationIdAccessor;
            this.serviceProvider = serviceProvider;
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            this.contextAccessor.Context = new SecurityContext(context.MessageId.ToString(), context.Message.UserContext);

            this.correlationIdAccessor.Id = context.Message.CorrelationId;
            this.correlationIdAccessor.ScopeId = context.Message.ScopeCorrelationId;

            // If it is a saga let it through. Short-Circuit the filter.
            if (IsSaga(context))
            {
                this.logger.LogInformation($"Beginning handling Saga message type: {typeof(T)} with corellation id: {context.Message.CorrelationId}");
                await next.Send(context);
                this.logger.LogInformation($"End handling Saga message type: {typeof(T)} with corellation id: {context.Message.CorrelationId}");
                return;
            }

            try
            {
                this.logger.LogInformation($"Beginning handling message type: {typeof(T)} with corellation id: {context.Message.CorrelationId}");
                var paramType = context.Message.GetType();

                var handlerType = context.Message is Event ?
                    typeof(IEventHandlerAsync<>).MakeGenericType(paramType) :
                    typeof(ICommandHandlerAsync<>).MakeGenericType(paramType);

                var handler = this.serviceProvider.GetService(handlerType);

                var response = (Task)handler.GetType().GetMethod(HandlerMethodName)?.Invoke(handler, new object[] { context.Message });

                if (response != null)
                {
                    await response;
                }

                this.logger.LogInformation($"End handling message type: {typeof(T)} with corellation id: {context.Message.CorrelationId}");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                throw;
            }
        }

        public void Probe(ProbeContext context)
        {
        }

        private static bool IsSaga(ISendObserverConnector context)
        {
            return context.GetType().GetGenericTypeDefinition() == typeof(DefaultSagaConsumeContext<,>)
                || context.GetType().GetGenericTypeDefinition() == typeof(InMemorySagaConsumeContext<,>);
        }
    }
}
