using FluentValidation;
using SpendingsManagementWebAPI.Dtos.GroupDtos;

namespace SpendingsManagementWebAPI.Validators.GroupValidators
{
    public class EditGroupValidator : AbstractValidator<EditGroupDto>
    {
        public EditGroupValidator(SpendingDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .Custom((value, context) =>
                {
                    if (value != null)
                    {
                        var nameInUse = dbContext.SpendingGroups.Any(g => g.Name == value);


                        if (nameInUse)
                        {
                            context.AddFailure("Group", "Istnieje juz grupa o takiej nazwie.");
                        }
                    }
                });
        }
    }
}
