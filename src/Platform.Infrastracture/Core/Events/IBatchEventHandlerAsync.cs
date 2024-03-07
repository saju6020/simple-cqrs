namespace Platform.Infrastructure.Core.Events
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBatchEventHandlerAsync<in TEvent>
        where TEvent : IEvent
    {
        Task HandleAsync(IEnumerable<TEvent> @event);
    }
}
