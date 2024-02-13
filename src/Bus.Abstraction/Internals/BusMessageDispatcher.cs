// <copyright file="BusMessageDispatcher.cs" company="Shohoz">
// Copyright © 2015-2020 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System;
    using System.Threading.Tasks;
    using Shohoz.Platform.Infrastructure.Core.Bus;

    public class BusMessageDispatcher : IBusMessageDispatcher
    {
        private readonly IBusProvider busProvider;

        public BusMessageDispatcher(IBusProvider busProvider)
        {
            this.busProvider = busProvider;
        }

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
    }
}