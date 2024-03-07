namespace Platform.Infrastructure.Bus.Abstraction
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Commands;

    [ExcludeFromCodeCoverage]
    public class MassTransitBatchCommandConsumerAsync<TCommand> : IConsumer<Batch<TCommand>>, IConsumer
        where TCommand : Command
    {
        public Task Consume(ConsumeContext<Batch<TCommand>> context)
        {
            return Task.CompletedTask;
        }
    }
}
