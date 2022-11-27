using CarRepairServiceCode.RequestModels.Authorization;
using FluentValidation;

namespace CarRepairServiceCode.Validation
{
    public class AuthValidator : AbstractValidator<AuthRequest>
    {
        public AuthValidator()
        {
            RuleFor(x => x.EmpLogin).NotNull().Length(10, 50).EmailAddress();
            RuleFor(x => x.EmpPassword).NotNull().PasswordCustomValidator();
        }
    }
}
