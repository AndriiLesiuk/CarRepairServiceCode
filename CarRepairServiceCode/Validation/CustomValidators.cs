using FluentValidation;
using FluentValidation.Validators;

namespace CarRepairServiceCode.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> PasswordCustomValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"^(?=.*[a-z])(?=.*\d).{6,20}$")).WithMessage(Helper.Messages.PasswordCustomValidatorMessage);
        }
    }
}
