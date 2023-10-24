using System.ComponentModel.DataAnnotations;

namespace SimpleCQRS.Blog.Dtos
{
    public class CommentDto
    {
        [Key]
        public Guid CommentId { get; set; }

        public string? CommentContents { get; set; }

        public string? CommentedBy { get; set; }
      
    }
}
