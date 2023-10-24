namespace Platform.Infrastructure.Core.Accessors
{
    using Platform.Infrastructure.Core.Models;

    public interface IContextAccessor
    {
        SecurityContext Context { get; set; }
    }
}
