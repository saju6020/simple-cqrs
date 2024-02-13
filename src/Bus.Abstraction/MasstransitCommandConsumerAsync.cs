namespace Platform.Infrastructure.Bus.Abstraction
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Bus;

    [ExcludeFromCodeCoverage]
    public class MassTransitCommandConsumerAsync<TCommand> : IConsumer<TCommand>, IConsumer
        where TCommand : Message
    {
        public Task Consume(ConsumeContext<TCommand> context)
        {
            return Task.CompletedTask;
        }
    }
}
