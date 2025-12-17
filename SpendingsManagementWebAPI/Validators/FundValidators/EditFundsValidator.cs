using FluentValidation;
using SpendingsManagementWebAPI.Dtos.FundsDtos;

namespace SpendingsManagementWebAPI.Validators.FundValidators
{
    public class EditFundsValidator : AbstractValidator<EditFundDto>
    {
        public EditFundsValidator()
        {
            RuleFor(x => x.AmountAdded)
                .GreaterThanOrEqualTo(0);

        }
    }
}
