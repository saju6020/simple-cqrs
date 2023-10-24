namespace Platform.Infrastructure.Core.Bus
{
    using System;
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Events;

    /// <summary>Event Handler Abastraction Class.</summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <seealso cref="Platform.Infrastructure.Core.Events.IEventHandlerAsync{T}" />
    public abstract class EventHandlerAsync<T> : IEventHandlerAsync<T>
        where T : class, IEvent
    {
        protected EventHandlerAsync(IServiceProvider serviceProvider)
        {
        }

        public abstract Task HandleAsync(T @event);
    }
}
