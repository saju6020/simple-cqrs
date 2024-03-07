using FluentValidation;
using SimpleCQRS.UAM.Common;
using SimpleCQRS.UAM.Domain.Commands;
using System.Text.RegularExpressions;

namespace SimpleCQRS.UAM.Domain.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private static string userNameValidationRegex = @"^[\S]*$";
        public CreateUserCommandValidator()
        {
            this.RuleFor(u => u.UserName).NotEmpty().WithMessage(UAMConstants.USERNAME_IS_REQUIRED);
            this.RuleFor(u => u.Email).NotEmpty().WithMessage(UAMConstants.EMAIL_IS_REQUIRED);
            this.RuleFor(u => u.UserName).Must(this.ValidUserName).WithMessage(UAMConstants.INVALID_USERNAME_FORMAT);
            this.RuleFor(u => u.Email).EmailAddress().WithMessage(UAMConstants.VALID_EMAIL_IS_REQUIRED);
            
        }

        private bool ValidUserName(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                return Regex.IsMatch(userName, userNameValidationRegex);
            }

            return true;
        }
    }
}
