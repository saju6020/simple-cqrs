namespace Platform.Infrastructure.NoSql.Repository.Contracts.Models
{
    /// <summary>This class is to contain pagination information.</summary>
    public class DataPagination
    {
        /// <summary>Initializes a new instance of the <see cref="DataPagination"/> class.</summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public DataPagination(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;

            this.PageSize = pageSize;
        }

        /// <summary>Gets or sets the page number.</summary>
        /// <value>The page number.</value>
        public int PageNumber { get; set; }

        /// <summary>Gets or sets the size of the page.</summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }
    }
}