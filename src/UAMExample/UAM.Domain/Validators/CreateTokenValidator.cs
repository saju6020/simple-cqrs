using FluentValidation;
using SimpleCQRS.UAM.Common;
using SimpleCQRS.UAM.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRS.UAM.Domain.Validators
{
    public class CreateTokenValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenValidator() {

            this.RuleFor(t => t.UserName).NotEmpty().NotNull().WithMessage(UAMConstants.USERNAME_IS_REQUIRED);
            this.RuleFor(t => t.Password).NotEmpty().NotNull().WithMessage(UAMConstants.PASSWORD_IS_REQUIRED);
        }
    }
}
