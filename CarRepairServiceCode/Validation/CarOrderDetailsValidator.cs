using CarRepairServiceCode.RequestModels.CarOrderDetail;
using FluentValidation;

namespace CarRepairServiceCode.Validation
{
    public class CarOrderDetailsValidator : AbstractValidator<CarOrderDetailRequest>
    {
        public CarOrderDetailsValidator()
        {
            RuleFor(x => x.TaskId).NotNull();
            RuleFor(x => x.EmployeeId).NotNull();
        }
    }
}
