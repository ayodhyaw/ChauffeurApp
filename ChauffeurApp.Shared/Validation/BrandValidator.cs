using ChauffeurApp.Core.Entities;
using FluentValidation;

namespace ChauffeurApp.Shared.Validation
{
    public class BrandValidator: AbstractValidator<Brand>
    {
        public BrandValidator()
        {
            RuleFor(u => u.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(u => u.Features).NotNull().NotEmpty().WithMessage("Features must not be empty");
        }
    }
}
