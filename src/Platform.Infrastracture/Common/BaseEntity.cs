using System.ComponentModel.DataAnnotations;

namespace Platform.Infrastructure.Common
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? PublishedOn { get; set; }
    }
}
