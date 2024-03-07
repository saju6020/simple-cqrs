namespace Platform.Infrastructure.Core.Validation
{
    /// <summary>Validation Options Model.</summary>
    public class ValidationOptions
    {
        public bool ValidateAllCommands { get; set; } = false;

        public bool ValidateAllQuery { get; set; }
    }
}
