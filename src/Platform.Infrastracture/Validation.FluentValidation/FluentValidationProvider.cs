namespace Platform.Infrastructure.Validation.FluentValidationProvider
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Results;
    using global::Platform.Infrastructure.Core.Commands;
    using global::Platform.Infrastructure.Core.Dependencies;
    using global::Platform.Infrastructure.Core.Validation;

    public class FluentValidationProvider : IValidationProvider
    {
        private readonly IHandlerResolver handlerResolver;

        public FluentValidationProvider(IHandlerResolver handlerResolver)
        {
            this.handlerResolver = handlerResolver;
        }

        public async Task<ValidationResponse> ValidateAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var validator = this.handlerResolver.ResolveHandler(command, typeof(IValidator<>));
            var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { command.GetType(), typeof(CancellationToken) });
            var validationResult = await ((Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { command, new CancellationTokenSource().Token }))
                                    .ConfigureAwait(false);

            return BuildValidationResponse(validationResult);
        }

        public ValidationResponse Validate<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var validator = this.handlerResolver.ResolveHandler(command, typeof(IValidator<>));
            var validateMethod = validator.GetType().GetMethod("Validate", new[] { command.GetType() });
            var validationResult = (ValidationResult)validateMethod.Invoke(validator, new object[] { command });

            return BuildValidationResponse(validationResult);
        }

        private static ValidationResponse BuildValidationResponse(
           ValidationResult validationResult)
        {
            return new ValidationResponse
            {
                Errors = validationResult.Errors.Select(failure => new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage,
                    ErrorCode = failure.ErrorCode,
                }).ToList(),
            };
        }

        public async Task<ValidationResponse> ValidateQueryAsync<TQuery>(TQuery query)
            where TQuery : class
        {
            var validator = this.handlerResolver.ResolveHandler(query, typeof(IValidator<>));
            var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { query.GetType(), typeof(CancellationToken) });
            var validationResult = await ((Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { query, new CancellationTokenSource().Token }))
                                    .ConfigureAwait(false);

            return BuildValidationResponse(validationResult);
        }

        public ValidationResponse ValidateQuery<TQuery>(TQuery query)
            where TQuery : class
        {
            var validator = this.handlerResolver.ResolveHandler(query, typeof(IValidator<>));
            var validateMethod = validator.GetType().GetMethod("Validate", new[] { query.GetType() });
            var validationResult = (ValidationResult)validateMethod.Invoke(validator, new object[] { query });

            return BuildValidationResponse(validationResult);
        }
    }
}
