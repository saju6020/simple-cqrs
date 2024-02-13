namespace Platform.Infrastructure.Bus.Abstraction
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Bus;

    [ExcludeFromCodeCoverage]
    public class MassTransitEventConsumerAsync<TCommand> : IConsumer<TCommand>, IConsumer
        where TCommand : Message
    {
        public Task Consume(ConsumeContext<TCommand> context)
        {
            return Task.CompletedTask;
        }
    }
}
