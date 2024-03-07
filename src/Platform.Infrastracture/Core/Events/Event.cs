namespace Platform.Infrastructure.Core.Events
{
    using System;
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Event Model.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.BusTopicMessage" />
    /// <seealso cref="Platform.Infrastructure.Core.Events.IEvent" />
    public class Event : BusTopicMessage, IEvent
    {
        public string Source { get; set; }
    }
}
