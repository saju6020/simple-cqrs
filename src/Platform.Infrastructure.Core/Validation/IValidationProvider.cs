namespace Platform.Infrastructure.Core.Validation
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Commands;

    /// <summary>Validation Provider Interface.</summary>
    public interface IValidationProvider
    {
        Task<ValidationResponse> ValidateAsync<TCommand>(TCommand command)
           where TCommand : ICommand;

        ValidationResponse Validate<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task<ValidationResponse> ValidateQueryAsync<TQuery>(TQuery query)
          where TQuery : class;

        ValidationResponse ValidateQuery<TQuery>(TQuery query)
            where TQuery : class;
    }
}
