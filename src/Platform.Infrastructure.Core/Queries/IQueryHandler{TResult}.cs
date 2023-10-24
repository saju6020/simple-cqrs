namespace Platform.Infrastructure.Core.Queries
{
    /// <summary>Query Handler Interface.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<TResult>
    {
        QueryResponse<TResult> Handle();
    }
}
