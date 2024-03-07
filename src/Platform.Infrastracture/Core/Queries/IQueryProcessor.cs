namespace Platform.Infrastructure.Core.Queries
{
    using System.Threading.Tasks;

    /// <summary>QueryProcessor interface.</summary>
    public interface IQueryProcessor
    {
        Task<QueryResponse<TResult>> ProcessAsync<TResult>(IQuery<TResult> query);

        Task<QueryResponse<TResult>> ProcessAsync<TResult>();

        QueryResponse<TResult> Process<TResult>(IQuery<TResult> query);

        QueryResponse<TResult> Process<TResult>();
    }
}
