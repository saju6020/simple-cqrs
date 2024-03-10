namespace GenericCommandWeb.Domain
{
    public class CommandExecutionService : ICommandExecutionService
    {
        public CommandExecutionService(IAuthorizationService authorizationService) { 

        }
        public async Task ExecuteAsync(ClientCommand command)
        {

        }
    }
}
