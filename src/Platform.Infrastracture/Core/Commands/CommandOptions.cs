namespace Platform.Infrastructure.Core.Commands
{
    public class CommandOptions
    {
        public bool PublishEvents { get; set; } = false;
    
        public bool SaveCommandData { get; set; } = false;
    }
}
