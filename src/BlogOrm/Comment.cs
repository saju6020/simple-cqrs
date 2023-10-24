using System.ComponentModel.DataAnnotations;

namespace Blog.ORM.Models
{
    public class Comment : BaseEntity
    {
        [Key]
        public Guid CommentId { get; set; }

        public string? CommentContents { get; set; }

        public string? CommentedBy { get; set; }

        public Guid BlogId { get; set; }

        public BlogDetails Blog { get; set; }
    }
}
