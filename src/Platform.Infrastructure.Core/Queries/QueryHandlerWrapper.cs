namespace Platform.Infrastructure.Core.Queries
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Dependencies;

    /// <summary>Query Handler Wrapper Class.</summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="Platform.Infrastructure.Core.Queries.BaseQueryHandlerWrapper{TResult}" />
    internal class QueryHandlerWrapper<TQuery, TResult> : BaseQueryHandlerWrapper<TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>Handles the asynchronous.</summary>
        /// <param name="query">The query.</param>
        /// <param name="handlerResolver">The handler resolver.</param>
        /// <returns>Passed Type.</returns>
        public override Task<QueryResponse<TResult>> HandleAsync(IQuery<TResult> query, IHandlerResolver handlerResolver)
        {
            var handler = GetHandler<IQueryHandlerAsync<TQuery, TResult>>(handlerResolver);
            return handler.HandleAsync((TQuery)query);
        }

        /// <summary>Handles the specified query.</summary>
        /// <param name="query">The query.</param>
        /// <param name="handlerResolver">The handler resolver.</param>
        /// <returns>Passed Type.</returns>
        public override QueryResponse<TResult> Handle(IQuery<TResult> query, IHandlerResolver handlerResolver)
        {
            var handler = GetHandler<IQueryHandler<TQuery, TResult>>(handlerResolver);
            return handler.Handle((TQuery)query);
        }
    }
}
