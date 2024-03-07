namespace Platform.Infrastructure.Core.Events
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>Event Publisher Interface.</summary>
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;

        Task PublishAsync<TEvent>(IEnumerable<TEvent> events)
            where TEvent : IEvent;
    }
}
