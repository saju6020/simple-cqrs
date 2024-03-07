namespace Platform.Infrastructure.Core.Commands
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Commands;

    public interface ICommandHandlerAsync<in TCommand>
        where TCommand : ICommand
    {
        Task<CommandResponse> HandleAsync(TCommand command);
    }
}
