using Platform.Infrastructure.Core.Commands;

namespace Commands
{
    public class CreateProductCommand : Command
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid ProductId { get; set; }
    }
}

