using System.ComponentModel.DataAnnotations;

namespace Blog.ORM.Models
{
    public class Tag : BaseEntity
    {
        [Key]
        public Guid TagId { get; set; }

        public string? Title { get; set;}

        public string? Description { get; set; }

    }
}
