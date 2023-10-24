using System.ComponentModel.DataAnnotations;

namespace Blog.ORM.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public Guid CategoryId { get; set; }

        public string? Title { get; set;}

        public string? Description { get; set; }

        public List<BlogDetails>? Blogs { get; set; }

    }
}
