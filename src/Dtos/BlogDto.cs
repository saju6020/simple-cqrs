using System.ComponentModel.DataAnnotations;

namespace SimpleCQRS.Blog.Dtos
{
    public class BlogDto
    {
        [Key]
        public Guid PostId { get; set; }

        public string? Title { get; set; }

        public string? Slug { get; set; }

        public string? Summary { get; set; }

        public string? PostContents { get; set; }

        public Guid CategoryId { get; set; }
       
    }
}
