namespace Platform.Infrastructure.Core.Queries
{
    /// <summary>Query Handler Interface.</summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        QueryResponse<TResult> Handle(TQuery query);
    }
}
