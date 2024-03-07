namespace Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core;
    using Platform.Infrastructure.Core.Accessors;
    using Platform.Infrastructure.Core.Bus;

    public class MassTransitGenericSendFilter<T> : IFilter<SendContext<T>>
        where T : class
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IMessageCorrelationIdProvider messageCorrelationIdProvider;
        private readonly ILogger<MassTransitGenericSendFilter<T>> logger;

        public MassTransitGenericSendFilter(
            IUserContextProvider userContextProvider,
            ILogger<MassTransitGenericSendFilter<T>> logger,
            IMessageCorrelationIdProvider messageCorrelationIdProvider)
        {
            this.logger = logger;
            this.userContextProvider = userContextProvider;
            this.messageCorrelationIdProvider = messageCorrelationIdProvider;
        }

        public Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            if (IsFault())
            {
                return Task.CompletedTask;
            }

            IMessage contextMessage = context.Message as IMessage;

            Guid messageCorrelationId = this.messageCorrelationIdProvider.GetMessageCorrelationId();
            contextMessage.CorrelationId = messageCorrelationId != Guid.Empty ? messageCorrelationId : contextMessage.CorrelationId;

            if (contextMessage.UserContext == null)
            {
                UserContext userContext = this.userContextProvider.GetUserContext();
                contextMessage.SetUserContext(userContext);
            }

            this.logger.LogInformation($"Sending message of type: {context.Message.GetType().FullName} to queue: {context.DestinationAddress.AbsolutePath} with corellation id: {contextMessage.CorrelationId}");

            return Task.CompletedTask;

            // return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
        }

        private static bool IsFault()
        {
            return typeof(T).Equals(typeof(Fault));
        }
    }
}
