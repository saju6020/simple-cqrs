namespace Platform.Infrastructure.Core.Events
{
    using Platform.Infrastructure.Core.Bus;

    public class InMemoryEvent : Message, IEvent
    {
        public string Source { get; set; }
    }

}
