namespace GenericCommandWeb.Domain
{
    public interface ICommandExecutionService
    {

        public Task ExecuteAsync(ClientCommand command);
       
    }
}
