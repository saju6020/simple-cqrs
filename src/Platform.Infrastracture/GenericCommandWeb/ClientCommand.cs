using Platform.Infrastructure.Core.Commands;

namespace GenericCommandWeb
{
    public class ClientCommand
    {
        public Guid CorrelationId { get; set; }
        public string CommandType { get; set; }

        public object? Command {  get; set; }
    }
}
