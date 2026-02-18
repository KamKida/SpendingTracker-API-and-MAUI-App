using FluentValidation;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Infrastructure.Context;

namespace SpendingTracker.Infrastructure.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator(SpendingTrackerDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Należy podać adres e-mail.")
                .EmailAddress()
                .WithMessage("Należy podać adres e-mail.");


            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("Hasło musi mieć co najmniej sześć znaków.");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Wybrany email już posiada konto.");
                    }
                });
        }
    }
}
