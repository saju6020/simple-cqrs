namespace Platform.Infrastructure.Core.Commands
{
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Command abstraction.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.BusQueueMessage" />
    /// <seealso cref="Platform.Infrastructure.Core.Commands.ICommand" />
    public abstract class Command : BusQueueMessage, ICommand
    {
        public bool? PublishEvents { get; set; } = false;

        public bool? ValidateCommand { get; set; }
    }
}
