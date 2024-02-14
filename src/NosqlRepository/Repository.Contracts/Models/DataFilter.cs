namespace Platform.Infrastructure.NoSql.Repository.Contracts.Models
{
    /// <summary>Operator type enum for mongo aggregate query.</summary>
    public enum OperatorType
    {
        Eq,
        Neq,
        Lt,
        Gt,
        Lte,
        Gte,
        Regex,
    }

    /// <summary>For data filter.</summary>
    public class DataFilter
    {
        /// <summary>Initializes a new instance of the <see cref="DataFilter"/> class.</summary>
        public DataFilter()
        {
            this.Operator = OperatorType.Eq;
        }

        /// <summary>Gets or sets the property.</summary>
        /// <value>The property.</value>
        public string Property { get; set; }

        /// <summary>Gets or sets the operator.</summary>
        /// <value>The operator.</value>
        public OperatorType Operator { get; set; }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}
