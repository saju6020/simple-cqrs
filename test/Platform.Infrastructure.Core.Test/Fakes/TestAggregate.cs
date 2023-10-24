namespace Core.UnitTest.Fakes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Platform.Infrastructure.Core.Domain;

    public class TestAggregate : AggregateRoot
    {
        public int Number { get; private set; }

        private readonly List<Item> lineItems = new List<Item>();

        public ReadOnlyCollection<Item> Items => this.lineItems.AsReadOnly();

        public TestAggregate()
        {
        }

        public TestAggregate(int number)
        {
            this.AddAndApplyEvent<TestAggregate>(new TestAggregateCreated(number));
        }

        private void Apply(TestAggregateCreated @event)
        {
            this.Number = @event.Number;
        }

        public void AddItem(string v1, int v2, double v3, bool v4)
        {
            this.AddAndApplyEvent<TestAggregate>(new ItemAdded()
            {
                AggregateRootId = this.Id,
                Description = v1,
                Quantity = v2,
                Price = v3,
                Taxable = v4,
            });
        }

        private void Apply(ItemAdded @event)
        {
            this.lineItems.Add(new Item(this.Version, @event.Description, @event.Quantity, @event.Price, @event.Taxable));
        }

        public void RemoveItem(int id)
        {
            this.AddAndApplyEvent<TestAggregate>(new ItemRemoved()
            {
                AggregateRootId = this.Id,
                ItemId = id,
            });
        }

        private void Apply(ItemRemoved @event)
        {
            var item = this.lineItems.SingleOrDefault(l => l.Id == @event.ItemId);
            if (item != null)
            {
                this.lineItems.Remove(item);
            }
        }

        public void ChangeItem(int id, string description)
        {
            this.AddAndApplyEvent<TestAggregate>(new ItemDescriptionChanged()
            {
                AggregateRootId = this.Id,
                ItemId = id,
                Description = description,
            });
        }

        private void Apply(ItemDescriptionChanged @event)
        {
            var item = this.lineItems.SingleOrDefault(l => l.Id == @event.ItemId);
            if (item != null)
            {
                this.lineItems.Remove(item);
            }

            this.lineItems.Add(new Item(item.Id, @event.Description, item.Quantity, item.Price, item.Taxable));
        }

        public void ChangeItem(int id, int quantity)
        {
            this.AddAndApplyEvent<TestAggregate>(new ItemQuantityChanged()
            {
                AggregateRootId = this.Id,
                ItemId = id,
                Quantity = quantity,
            });
        }

        private void Apply(ItemQuantityChanged @event)
        {
            var item = this.lineItems.SingleOrDefault(l => l.Id == @event.ItemId);
            if (item != null)
            {
                this.lineItems.Remove(item);
            }

            this.lineItems.Add(new Item(item.Id, item.Description, @event.Quantity, item.Price, item.Taxable));
        }
    }
}
