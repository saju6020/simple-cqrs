namespace Platform.Infrastructure.Core.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Commands;

    public interface IBatchCommandHandlerAsync<in TCommand>
        where TCommand : ICommand
    {
        Task<List<CommandResponse>> HandleAsync(IEnumerable<TCommand> commands);
    }
}
