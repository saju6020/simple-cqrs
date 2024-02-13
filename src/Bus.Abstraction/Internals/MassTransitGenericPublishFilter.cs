namespace Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Accessors;
    using Platform.Infrastructure.Core.Bus;

    public class MassTransitGenericPublishFilter<T> : IFilter<PublishContext<T>>
        where T : class
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IMessageCorrelationIdProvider messageCorrelationIdProvider;
        private readonly ILogger<MassTransitGenericPublishFilter<T>> logger;

        public MassTransitGenericPublishFilter(
            IUserContextProvider userContextProvider,
            ILogger<MassTransitGenericPublishFilter<T>> logger,
            IMessageCorrelationIdProvider messageCorrelationIdProvider)
        {
            this.logger = logger;
            this.userContextProvider = userContextProvider;
            this.messageCorrelationIdProvider = messageCorrelationIdProvider;
        }

        public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
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

            // return next.Send(context);
            return Task.CompletedTask;
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
