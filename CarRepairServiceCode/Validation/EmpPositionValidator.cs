using CarRepairServiceCode.RequestModels.EmpPosition;
using FluentValidation;

namespace CarRepairServiceCode.Validation
{
    public class EmpPositionValidator : AbstractValidator<EmpPositionRequest>
    {
        public EmpPositionValidator()
        {
            RuleFor(x => x.PositionName).NotNull().Length(5, 30);
        }
    }
}
