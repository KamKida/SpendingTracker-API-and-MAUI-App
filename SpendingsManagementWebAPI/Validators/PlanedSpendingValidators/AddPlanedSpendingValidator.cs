using FluentValidation;
using SpendingsManagementWebAPI.Dtos.PlanedSpendingDtos;

namespace SpendingsManagementWebAPI.Validators.PlanedSpendingValidators
{
    public class AddPlanedSpendingValidator : AbstractValidator<AddPlanedSpendingDto>
    {
        public AddPlanedSpendingValidator(SpendingDbContext dbContext)
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x)
                .Must(x =>
                (x.AtWhichPointOfMonth != null && x.WhichDayOfMonth == null) ||
                (x.AtWhichPointOfMonth == null && x.WhichDayOfMonth != null))
            .WithMessage("Można określic tylko początek, koniec ('AtWhichPointOfMonth') lub okres miesiąca ('WhichDayOfMonth').");

            RuleFor(x => x.Name)
                .Custom((value, context) =>
                {
                    var groupNameInUse = dbContext.PlanedSpendings.Any(g => g.Name == value.Trim());

                    if (groupNameInUse)
                    {
                        context.AddFailure("PlanedSpending", "Istnieje juz zaplanowany wydatek o takiej nazwie.");
                    }
                });

        }
    }
}
