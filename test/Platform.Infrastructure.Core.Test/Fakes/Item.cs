namespace Core.UnitTest.Fakes
{
    using System;

    public class Item
    {
        public Item(int id, string description, int quantity, double price, bool taxable)
        {
            this.Id = id;
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
            this.Quantity = quantity;
            this.Price = price;
            this.Taxable = taxable;
        }

        public int Id { get; }

        public string Description { get; }

        public int Quantity { get; }

        public double Price { get; }

        public bool Taxable { get; }
    }
}
