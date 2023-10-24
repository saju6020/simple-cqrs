using System.ComponentModel.DataAnnotations;

namespace SimpleCQRS.Blog.Dtos
{
    public class CategoryDto
    {
        [Key]
        public Guid CategoryId { get; set; }

        public string? Title { get; set;}

        public string? Description { get; set; }      
    }
}
