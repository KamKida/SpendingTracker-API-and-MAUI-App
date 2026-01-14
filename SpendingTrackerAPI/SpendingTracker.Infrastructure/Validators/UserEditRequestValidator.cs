using FluentValidation;
using SpendingTracker.Contracts.Dtos.Requests;

namespace SpendingTracker.Infrastructure.Validators
{
    public class UserEditRequestValidator : AbstractValidator<UserRequest>
    {
        public UserEditRequestValidator(bool checkPassword = true)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Należy podać adres e-mail.")
                .EmailAddress()
                .WithMessage("Należy podać adres e-mail.");

			When(_ => checkPassword, () =>
			{
				RuleFor(x => x.Password)
					.NotEmpty()
					.WithMessage("Hasło jest wymagane.")
					.MinimumLength(6)
					.WithMessage("Hasło musi mieć co najmniej 6 znaków.");
			});
		}

    }
}
