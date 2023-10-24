namespace Core.UnitTest.Fakes
{
    using Platform.Infrastructure.Core.Domain;

    public class TestAggregateCreated : DomainEvent
    {
        public int Number { get; set; }

        public TestAggregateCreated(int number) => this.Number = number;
    }
}
