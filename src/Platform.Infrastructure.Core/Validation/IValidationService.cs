namespace Platform.Infrastructure.Core.Validation
{
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Queries;

    /// <summary>Validation Service Interface.</summary>
    public interface IValidationService
    {

        Task<ValidationResponse> ValidateAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        void Validate<TCommand>(TCommand command)
            where TCommand : ICommand;

        void ValidateQuery<TQuery>(TQuery query)
            where TQuery : class;

        Task<ValidationResponse> ValidateQueryAsync<TQuery>(TQuery query)
            where TQuery : class;
    }
}
