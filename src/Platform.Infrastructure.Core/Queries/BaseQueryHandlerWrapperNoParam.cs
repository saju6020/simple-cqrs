namespace Platform.Infrastructure.Core.Queries
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Dependencies;

    /// <summary>Base query handler wrapper.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal abstract class BaseQueryHandlerWrapperNoParam<TResult>
    {
        /// <summary>Gets the handler.</summary>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="handlerResolver">The handler resolver.</param>
        /// <returns>Handler.</returns>
        protected static THandler GetHandler<THandler>(IHandlerResolver handlerResolver)
        {
            return handlerResolver.ResolveHandler<THandler>();
        }

        /// <summary>Handles the asynchronous.</summary>
        /// <param name="serviceFactory">The service factory.</param>
        /// <returns>Passed Type.</returns>
        public abstract Task<QueryResponse<TResult>> HandleAsync(IHandlerResolver serviceFactory);

        public abstract QueryResponse<TResult> Handle(IHandlerResolver serviceFactory);
    }
}
