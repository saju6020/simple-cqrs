namespace Platform.Infrastructure.Core.Commands
{
    using Platform.Infrastructure.Core;
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Command Interface.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.IMessage" />
    public interface ICommand : IMessage
    {
        public EventPublishOption EventPublishOption { get; set; }
        public bool IsInMemoryCommand { get; set; }

        bool? ValidateCommand { get; set; }
    }
}
