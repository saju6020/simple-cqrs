namespace Platform.Infrastructure.Core.Queries
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Platform.Infrastructure.Core.Dependencies;
    using Platform.Infrastructure.Core.Validation;

    /// <summary>Query Processor.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Queries.IQueryProcessor" />
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IHandlerResolver handlerResolver;
        private readonly ILogger logger;
        private readonly IValidationService validationService;
        private readonly IOptions<ValidationOptions> validationOptions;

        public QueryProcessor(
            IHandlerResolver handlerResolver,
            IValidationService validationService,
            IOptions<ValidationOptions> validationOptions,
            ILogger<QueryProcessor> logger)
        {
            this.handlerResolver = handlerResolver;
            this.logger = logger;
            this.validationService = validationService;
            this.validationOptions = validationOptions;
        }

        /// <summary>Processes the asynchronous.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>Return Type.</returns>
        /// <exception cref="System.ArgumentNullException">query.</exception>
        public async Task<QueryResponse<TResult>> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            QueryResponse<TResult> queryResponse = new QueryResponse<TResult>();

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (this.validationOptions.Value.ValidateAllQuery)
            {
                queryResponse.ValidationResult = await this.validationService.ValidateQueryAsync(query).ConfigureAwait(false);
                if (queryResponse.ValidationResult != null && !queryResponse.ValidationResult.IsValid)
                {
                    return queryResponse;
                }
            }

            var queryType = query.GetType();
            var handler = (BaseQueryHandlerWrapper<TResult>)Activator.CreateInstance(typeof(QueryHandlerWrapper<,>).MakeGenericType(queryType, typeof(TResult)));
            var response = await handler.HandleAsync(query, this.handlerResolver).ConfigureAwait(false);

            return new QueryResponse<TResult>(response.ValidationResult != null ? response.ValidationResult : new ValidationResponse(), response.Result);
        }

        /// <summary>Processes the asynchronous.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>Return Type.</returns>
        /// <exception cref="System.ArgumentNullException">query.</exception>
        public async Task<QueryResponse<TResult>> ProcessAsync<TResult>()
        {
            var handler = (BaseQueryHandlerWrapperNoParam<TResult>)Activator.CreateInstance(typeof(QueryHandlerWrapper<>).MakeGenericType(typeof(TResult)));
            var response = await handler.HandleAsync(this.handlerResolver).ConfigureAwait(false);

            return new QueryResponse<TResult>(response.ValidationResult != null ? response.ValidationResult : new ValidationResponse(), response.Result);
        }

        /// <summary>Processes the specified query.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>Return passed type object.</returns>
        /// <exception cref="System.ArgumentNullException">query.</exception>
        public QueryResponse<TResult> Process<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (this.validationOptions.Value.ValidateAllQuery)
            {
                this.validationService.ValidateQuery(query);
            }

            var queryType = query.GetType();
            var handler = (BaseQueryHandlerWrapper<TResult>)Activator.CreateInstance(typeof(QueryHandlerWrapper<,>).MakeGenericType(queryType, typeof(TResult)));
            var response = handler.Handle(query, this.handlerResolver);

            return new QueryResponse<TResult>(response.ValidationResult != null ? response.ValidationResult : new ValidationResponse(), response.Result);
        }

        /// <summary>Processes the specified query.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>Return passed type object.</returns>
        /// <exception cref="System.ArgumentNullException">query.</exception>
        public QueryResponse<TResult> Process<TResult>()
        {
            var handler = (BaseQueryHandlerWrapperNoParam<TResult>)Activator.CreateInstance(typeof(QueryHandlerWrapper<>).MakeGenericType(typeof(TResult)));
            var response = handler.Handle(this.handlerResolver);

            return new QueryResponse<TResult>(response.ValidationResult != null ? response.ValidationResult : new ValidationResponse(), response.Result);
        }
    }
}