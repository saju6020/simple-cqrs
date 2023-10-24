namespace Core.UnitTest.Fakes
{
    using Platform.Infrastructure.Core.Domain;

    public class ItemDescriptionChanged : DomainEvent
    {
        public int ItemId { get; set; }

        public string Description { get; set; }
    }
}
