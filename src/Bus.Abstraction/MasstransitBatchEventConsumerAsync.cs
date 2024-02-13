namespace Platform.Infrastructure.Bus.Abstraction
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Events;

    [ExcludeFromCodeCoverage]
    public class MassTransitBatchEventConsumerAsync<T> : IConsumer<Batch<T>>, IConsumer
        where T : class, IEvent
    {
        public Task Consume(ConsumeContext<Batch<T>> context)
        {
            return Task.CompletedTask;
        }
    }
}
