namespace Platform.Infrastructure.Bus.Abstraction
{
    using MassTransit;
    using Microsoft.Extensions.Options;
    using Platform.Infrastructure.Core.Bus;

    /// <summary>Bust Builder Interface.</summary>
    public interface IBusBuilder
    {
        IBusControl BuildBus(IOptions<BusOptions> busOptions);
    }
}
