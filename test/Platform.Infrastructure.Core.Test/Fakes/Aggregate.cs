namespace Core.UnitTest
{
    using Platform.Infrastructure.Core.Domain;

    /// <summary>Facke aggreagate root.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Domain.AggregateRoot" />
    public class Aggregate : AggregateRoot
    {
        public Aggregate()
        {
            this.AddAndApplyEvent<Aggregate>(new AggregateCreated());
        }

        private void Apply(AggregateCreated @event)
        {
        }
    }
}