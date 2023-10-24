namespace Core.UnitTest.Fakes
{
    using Platform.Infrastructure.Core.Domain;

    public class ItemQuantityChanged : DomainEvent
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }
    }
}
