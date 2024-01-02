using ChauffeurApp.Core.Entities;
using FluentValidation;

namespace ChauffeurApp.Shared.Validation
{
    public class UserValidator : AbstractValidator<ApplicationUser>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.PhoneNumber).MaximumLength(10);
        }
    }
}
