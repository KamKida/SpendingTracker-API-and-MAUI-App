using FluentValidation;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Infrastructure.Context;

namespace SpendingTracker.Infrastructure.Validators
{
	public class SpendingCategoryRequestValidator : AbstractValidator<SpendingCategoryRequest>
	{
		public SpendingCategoryRequestValidator(SpendingTrackerDbContext dbContext)
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Nazwa kategorii wydatków jest wymagana.");

			RuleFor(x => x.Name)
				.Custom((value, context) =>
				{
					var nameInUse = dbContext.SpendingCategories.Any(u => u.Name == value);

					if (nameInUse)
					{
						context.AddFailure("Name", "Kategoria wydatków o tej nazwie już istnieje.");
					}
				});
		}
	}
}
