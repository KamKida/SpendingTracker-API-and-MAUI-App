using FluentValidation;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;

namespace SpendingsManagementWebAPI.Validators.SpendingVlidators
{
    public class AddSpendingValidator : AbstractValidator<AddSpendingDto>
    {
        public AddSpendingValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThanOrEqualTo(0);

        }
    }
}
