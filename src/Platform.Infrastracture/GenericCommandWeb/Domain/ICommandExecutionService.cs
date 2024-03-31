using Platform.Infrastructure.Core.Commands;

namespace GenericCommandWeb.Domain
{
    public interface ICommandExecutionService
    {

        public Task<CommandResponse> ExecuteAsync(ClientCommand command);
       
    }
}
