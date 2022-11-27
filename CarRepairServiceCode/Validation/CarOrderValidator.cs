using CarRepairServiceCode.RequestModels.CarOrder;
using FluentValidation;

namespace CarRepairServiceCode.Validation
{
    public class CarOrderValidator : AbstractValidator<CarOrderRequest>
    {
        public CarOrderValidator()
        {
            RuleFor(x => x.CarId).NotNull();
            RuleFor(x => x.OrderComments).NotNull().Length(20, 500);
            RuleForEach(x => x.CarOrderDetails).SetValidator(new CarOrderDetailsValidator());
        }
    }
}
