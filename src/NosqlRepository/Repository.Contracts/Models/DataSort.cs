namespace Platform.Infrastructure.NoSql.Repository.Contracts.Models
{
    /// <summary>Enum to contain sort order.</summary>
    public enum SortOrderType
    {
        Ascending = 1,
        Descending = -1,
    }

    /// <summary>Class to handle data sort.</summary>
    public class DataSort
    {
        /// <summary>Initializes a new instance of the <see cref="DataSort"/> class.</summary>
        public DataSort()
        {
            this.Order = SortOrderType.Ascending;
        }

        /// <summary>Initializes a new instance of the <see cref="DataSort"/> class.</summary>
        /// <param name="property">The property.</param>
        /// <param name="order">The order.</param>
        public DataSort(string property, SortOrderType order = SortOrderType.Ascending)
        {
            this.Property = property;
            this.Order = order;
        }

        /// <summary>Gets or sets the property.</summary>
        /// <value>The property.</value>
        public string Property { get; set; }

        /// <summary>Gets or sets the order.</summary>
        /// <value>The order.</value>
        public SortOrderType Order { get; set; }
    }
}
