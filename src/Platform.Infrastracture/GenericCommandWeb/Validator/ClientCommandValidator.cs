using FluentValidation;

namespace GenericCommandWeb.Validator
{
    public class ClientCommandValidator : AbstractValidator<ClientCommand>
    {
        public ClientCommandValidator() {
            RuleFor(command => command.CorrelationId).NotEmpty().NotNull().WithMessage(Constants.CorrelationIdRequired);
            RuleFor(command=> command.CommandType).NotEmpty().WithMessage(Constants.CommandTypeRequired);
            RuleFor(command => command.Command).NotNull().WithMessage(Constants.CommandIsRequired);
        }
    }
}
