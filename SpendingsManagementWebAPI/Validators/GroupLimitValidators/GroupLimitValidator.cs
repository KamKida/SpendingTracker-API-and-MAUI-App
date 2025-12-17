using FluentValidation;
using SpendingsManagementWebAPI.Dtos.GroupLimitDtos;

namespace SpendingsManagementWebAPI.Validators.GroupLimitValidators
{
    public class GroupLimitValidator : AbstractValidator<GroupLimitDto>
    {
        public GroupLimitValidator(SpendingDbContext dbContext)
        {
            RuleFor(x => x.LimitPerSpending)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.TotalSpendingLimit)
                .GreaterThanOrEqualTo(1);

        }
    }
}
