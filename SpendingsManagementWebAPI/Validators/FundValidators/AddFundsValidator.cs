using FluentValidation;
using SpendingsManagementWebAPI.Dtos.FundsDtos;

namespace SpendingsManagementWebAPI.Validators.FundValidators
{
    public class AddFundsValidator : AbstractValidator<AddFundDto>
    {
        public AddFundsValidator()
        {
            RuleFor(f => f.AmountAdded)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
        }
    }
}

