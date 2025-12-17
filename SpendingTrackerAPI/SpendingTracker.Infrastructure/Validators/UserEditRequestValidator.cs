using FluentValidation;
using SpendingTracker.Contracts.Dtos.Requests;

namespace SpendingTracker.Infrastructure.Validators
{
    public class UserEditRequestValidator : AbstractValidator<UserEditRequest>
    {
        public UserEditRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Należy podać adres e-mail.")
                .EmailAddress()
                .WithMessage("Należy podać adres e-mail.");


            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("Hasło musi mieć co najmniej sześć znaków.");
        }

    }
}
