using Dtos;
using FluentValidation;

namespace Aggregates.Validators
{
    internal class CreateProductValidator : AbstractValidator<ProductDto>
    {
        public CreateProductValidator() {
            this.RuleFor(p => p.Title).NotEmpty().WithMessage("Title is required");
        }
    }
}
