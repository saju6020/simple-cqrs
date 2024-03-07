namespace Platform.Infrastructure.Core.Queries
{
    using System.Threading.Tasks;

    /// <summary>QueryHandlerAsync Interface.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandlerAsync<TResult>
    {
        Task<QueryResponse<TResult>> HandleAsync();
    }
}
