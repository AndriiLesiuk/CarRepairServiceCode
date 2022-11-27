using CarRepairServiceCode.RequestModels.TaskCatalog;
using FluentValidation;

namespace CarRepairServiceCode.Validation
{
    public class TaskCatalogValidator : AbstractValidator<TaskCatalogRequest>
    {
        public TaskCatalogValidator()
        {
            RuleFor(x => x.TaskName).NotNull().Length(10, 60);
            RuleFor(x => x.TaskDescription).Length(20, 500);
        }
    }
}
