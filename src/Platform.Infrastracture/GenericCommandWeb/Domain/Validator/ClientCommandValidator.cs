using FluentValidation;
using GenericCommandWeb.Domain;

namespace GenericCommandWeb.Domain.Validator
{
    public class ClientCommandValidator : AbstractValidator<CommandDto>
    {
        public ClientCommandValidator()
        {
            RuleFor(command => command.CorrelationId).NotEmpty().NotNull().WithMessage(Constants.CorrelationIdRequired);
            RuleFor(command => command.CommandType).NotEmpty().WithMessage(Constants.CommandTypeRequired);
            RuleFor(command => command.Command).NotNull().WithMessage(Constants.CommandIsRequired);
        }
    }
}
