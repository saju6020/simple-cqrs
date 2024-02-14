namespace Platform.Infrastructure.Bus.RabbitMQ
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Bus;

    /// <summary>
    ///   <br />
    /// </summary>
    /// <typeparam name="TEvent">The type of the command.</typeparam>
    [ExcludeFromCodeCoverage]
    public class EventConsumerAdapter<TEvent> : IConsumer<TEvent>, IConsumer
        where TEvent : Message
    {
        public Task Consume(ConsumeContext<TEvent> context)
        {
            return Task.CompletedTask;
        }
    }
}
