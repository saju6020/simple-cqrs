namespace Platform.Infrastructure.Core.Events
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Commands;

    /// <summary>Domain Event Processor Interface.</summary>
    public interface IDomainEventProcessor
    {
        Task Process(IEnumerable<IEvent> events, ICommand command);
    }
}
