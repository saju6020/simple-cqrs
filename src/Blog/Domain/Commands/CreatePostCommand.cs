using Platform.Infrastructure.Core.Commands;

namespace SimpleCQRS.Blog.Domain.Commands
{
    public class CreatePostCommand : Command
    {
        public Guid PostId { get; set; }

        public string? Title { get; set; }

        public string? Slug { get; set; }

        public string? Summary { get; set; }

        public string? PostContents { get; set; }

        public Guid CategoryId { get; set; }      
       
    }
}
