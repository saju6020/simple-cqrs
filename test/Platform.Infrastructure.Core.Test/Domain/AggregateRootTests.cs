namespace Core.UnitTest.Domain
{
    using System.Linq;
    using Core.UnitTest.Fakes;
    using Xunit;

    public class AggregateRootTests
    {
        [Fact]
        public void CreateTestAggregate()
        {
            var sut = new TestAggregate(10);
            Assert.Single(sut.Events);
            Assert.Equal(1, sut.Version);
            Assert.Equal(10, sut.Number);
        }

        [Fact]
        public void AddItem()
        {
            var sut = new TestAggregate(100);

            sut.AddItem("Description", 1, 10.00, true);

            Assert.Equal(2, sut.Events.Count);
            Assert.Equal(2, sut.Version);
            Assert.Single(sut.Items);
            Assert.Equal("Description", sut.Items[0].Description);
            Assert.Equal(1, sut.Items[0].Quantity);
            Assert.Equal(10.00, sut.Items[0].Price);
            Assert.True(sut.Items[0].Taxable);
        }

        [Fact]
        public void RemoveItem()
        {
            var sut = new TestAggregate(100);
            sut.AddItem("Description", 1, 10.00, true);

            sut.RemoveItem(1);

            var itemCount = sut.Items.Count;
            Assert.Equal(3, sut.Events.Count);
            Assert.Equal(3, sut.Version);
            Assert.Equal(0, itemCount);
        }

        [Fact]
        public void ChangeItem()
        {
            var sut = new TestAggregate(100);
            sut.AddItem("Description", 1, 10.00, true);

            sut.ChangeItem(1, "New Description");
            sut.ChangeItem(1, 10);

            Assert.Equal(4, sut.Events.Count);
            Assert.Equal(4, sut.Version);
            Assert.Single(sut.Items);
            Assert.Equal("New Description", sut.Items[0].Description);
            Assert.Equal(10, sut.Items[0].Quantity);
        }
    }
}
