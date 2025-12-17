using FluentValidation;
using SpendingsManagementWebAPI.Dtos.GroupDtos;

namespace SpendingsManagementWebAPI.Validators.GroupValidators
{
    public class AddGroupValidator : AbstractValidator<AddGroupDto>
    {
        public AddGroupValidator(SpendingDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .Custom((value, context) =>
                {
                    var groupNameInUse = dbContext.SpendingGroups.Any(g => g.Name == value.Trim());

                    if (groupNameInUse)
                    {
                        context.AddFailure("Group", "Istnieje juz grupa o takiej nazwie.");
                    }
                });
        }
    }
}
