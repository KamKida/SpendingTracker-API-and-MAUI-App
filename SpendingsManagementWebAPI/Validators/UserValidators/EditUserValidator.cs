using FluentValidation;
using SpendingsManagementWebAPI.Dtos.UserDtos;

namespace SpendingsManagementWebAPI.Validators.UserValidators
{
    public class EditUserValidator : AbstractValidator<EditUserDto>
    {
        public EditUserValidator(SpendingDbContext dbContext)
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty()
                .EmailAddress();


            RuleFor(x => x.NewPassword).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.NewPassword);

            RuleFor(x => x.NewEmail)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken.");
                    }
                });
        }
    }
}
