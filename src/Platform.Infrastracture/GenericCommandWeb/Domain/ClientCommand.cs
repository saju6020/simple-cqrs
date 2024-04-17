using Platform.Infrastructure.Core.Commands;

namespace GenericCommandWeb.Domain
{
    public class CommandDto
    {
        public Guid CorrelationId { get; set; }
        public string CommandType { get; set; }

        public object? Command { get; set; }
    }
}
