using FluentValidation;
using SpendingsManagementWebAPI.Dtos.SpendingLimitDtos;

namespace SpendingsManagementWebAPI.Validators.SpendingLimitValiddators
{
    public class EditSpendingLimitValidator : AbstractValidator<EditSpendingLimitDto>
    {
        public EditSpendingLimitValidator()
        {
            RuleFor(l => l.Limit)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);

            RuleFor(l => l.NumberOfDays)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        }
    }
}
