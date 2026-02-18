using FluentValidation;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Infrastructure.Context;

namespace SpendingTracker.Infrastructure.Validators
{
	public class FundCategoryRequestValidator : AbstractValidator<FundCategoryRequest>
    {
        public FundCategoryRequestValidator(SpendingTrackerDbContext dbContext)
	{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Nazwa jest wymagana.")
				.NotEmpty()
				.WithMessage("Nazwa jest wymagana.");



		RuleFor(x => x.Name)
			.Custom((value, context) =>
			{
				var emailInUse = dbContext.FundCategories.Any(u => u.Name == value);

				if (emailInUse)
				{
					context.AddFailure("Email", "Kategoria o tej nazwie już istnieje.");
				}
			});
	}
}
}
