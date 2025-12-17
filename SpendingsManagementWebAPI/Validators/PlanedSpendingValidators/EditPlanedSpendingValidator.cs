using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Dtos.PlanedSpendingDtos;

namespace SpendingsManagementWebAPI.Validators.PlanedSpendingValidators
{
    public class EditPlanedSpendingValidator : AbstractValidator<EditPlanedSpendingDto>
    {
        public EditPlanedSpendingValidator(SpendingDbContext dbContext)
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
                    if (value != null)
                    {
                        var nameInUse = dbContext.PlanedSpendings.Any(g => g.Name == value.Trim());


                        if (nameInUse)
                        {
                            context.AddFailure("PlanedSpending", "Istnieje juz zaplanowany wydatek o takiej nazwie.");
                        }
                    }
                });

            RuleFor(x => x.GroupId)
                .Custom((value, context) =>
                {
                    if (value != null)
                    {
                        var groupIdInUse = dbContext.PlanedSpendings.Any(g => g.GroupId == value);


                        if (groupIdInUse)
                        {
                            context.AddFailure("PlanedSpending", "Istnieje juz zaplanowany wydatek przypisany do tej grupy.");
                        }
                    }
                });
        }
    }
}
