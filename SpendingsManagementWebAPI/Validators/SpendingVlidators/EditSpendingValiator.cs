using FluentValidation;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;

namespace SpendingsManagementWebAPI.Validators.SpendingVlidators
{
    public class EditSpendingValiator : AbstractValidator<EditSpendingDto>
    {
        public EditSpendingValiator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThanOrEqualTo(0);

        }
    }
}
