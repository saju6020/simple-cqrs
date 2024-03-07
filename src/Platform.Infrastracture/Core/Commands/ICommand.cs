namespace Platform.Infrastructure.Core.Commands
{
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Command Interface.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.IMessage" />
    public interface ICommand : IMessage
    {
        bool? PublishEvents { get; set; }

        bool? ValidateCommand { get; set; }
    }
}
