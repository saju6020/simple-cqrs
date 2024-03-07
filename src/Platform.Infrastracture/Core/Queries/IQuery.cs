namespace Platform.Infrastructure.Core.Queries
{
    using Platform.Infrastructure.Common.Security;

    /// <summary>Query Interface.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="Platform.Infrastructure.Core.Security.ISecurityInfo" />
    public interface IQuery<TResult> : ISecurityInfo
    {
    }
}
