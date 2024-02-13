// <copyright file="MassTransitBus.cs" company="Shohoz">
// Copyright © 2015-2020 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Shohoz.Platform.Infrastructure.Core.Bus;
    using Shohoz.Platform.Infrastructure.CustomException;

    public class MassTransitBus : Core.Bus.IBus
    {
        private readonly IBusControl busControl;

        public MassTransitBus(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public Task PublishAsync<T>(T @event)
            where T : IBusTopicMessage
        {
            return this.busControl.Publish(@event);
        }

        public async Task SendAsync<T>(T command)
            where T : IBusQueueMessage
        {
            if (command.CorrelationId.Equals(Guid.Empty))
            {
                throw new ShohozBaseException("IBusQueueMessage or command must have CorrelationId");
            }

            var queueName = string.IsNullOrWhiteSpace(command.QueueName) == false ? command.QueueName : command.GetType().Namespace;

            var sendToUri = new Uri($"queue:{queueName}");

            var endPoint = await this.busControl.GetSendEndpoint(sendToUri);

            await endPoint.Send(command);
        }
    }
}
