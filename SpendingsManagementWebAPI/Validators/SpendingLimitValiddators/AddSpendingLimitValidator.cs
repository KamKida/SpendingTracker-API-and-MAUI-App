using FluentValidation;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;
using SpendingsManagementWebAPI.Dtos.SpendingLimitDtos;

namespace SpendingsManagementWebAPI.Validators.SpendingLimitValiddators
{
    public class AddSpendingLimitValidator : AbstractValidator<AddSpendingLimitDto>
    {
        public AddSpendingLimitValidator()
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
