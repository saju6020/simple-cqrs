namespace Platform.Infrastructure.Core.Commands
{
    using Platform.Infrastructure.Core;
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Command abstraction.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.BusQueueMessage" />
    /// <seealso cref="Platform.Infrastructure.Core.Commands.ICommand" />
    public abstract class Command : BusQueueMessage, ICommand
    {
        public EventPublishOption EventPublishOption { get; set; } = EventPublishOption.DoNotPublish;

        public bool IsInMemoryCommand { get; set; } = true;

        public bool? ValidateCommand { get; set; }
    }
}
