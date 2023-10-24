namespace Core.UnitTest.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Platform.Infrastructure.Core.Domain;

    public class ItemAdded : DomainEvent
    {
        public string Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public bool Taxable { get; set; }
    }
}
