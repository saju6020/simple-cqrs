namespace Platform.Infrastructure.Core.Events
{
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Event Interface.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.IMessage" />
    public interface IEvent : IMessage
    {
        string Source { get; set; }
    }
}
