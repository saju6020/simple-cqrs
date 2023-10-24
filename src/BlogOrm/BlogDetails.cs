using System.ComponentModel.DataAnnotations;

namespace Blog.ORM.Models
{
    public class BlogDetails: BaseEntity
    {
        [Key]
        public Guid BlogId { get; set; }

        public string? Title { get; set; }

        public string? Slug { get; set; }

        public string? Summary { get; set; }

        public string? BlogContents { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
