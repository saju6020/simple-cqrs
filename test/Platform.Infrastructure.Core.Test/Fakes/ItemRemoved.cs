namespace Core.UnitTest.Fakes
{
    using Platform.Infrastructure.Core.Domain;

    public class ItemRemoved : DomainEvent
    {
        public int ItemId { get; set; }
    }
}
