namespace Platform.Infrastructure.Core.Commands
{
    using System.Threading.Tasks;

    /// <summary>Command sender interface.</summary>
    public interface ICommandSender
    {
        Task<CommandResponse> SendAsync(ICommand command);

        CommandResponse Send(ICommand command);
    }
}
