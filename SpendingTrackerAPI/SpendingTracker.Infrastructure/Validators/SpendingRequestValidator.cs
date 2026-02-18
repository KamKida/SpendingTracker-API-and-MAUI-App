using FluentValidation;
using SpendingTracker.Contracts.Dtos.Requests;

namespace SpendingTracker.Infrastructure.Validators
{
	public class SpendingRequestValidator : AbstractValidator<SpendingRequest>
	{
		public SpendingRequestValidator()
		{
			RuleFor(f => f.Amount)
			.NotEmpty()
			.WithMessage("Kwota jest wymagana.")
			.GreaterThan(0)
			.WithMessage("Kwota nie może być mniejsza od zera.");
		}
	}
}