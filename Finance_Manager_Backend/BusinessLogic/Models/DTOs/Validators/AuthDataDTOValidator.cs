using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class AuthDataDTOValidator : AbstractValidator<AuthDataDTO>
{
    public AuthDataDTOValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty()
            .WithMessage("Email can't be empty")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(50)
            .WithMessage("Email must be less than 50 symbols.");

        RuleFor(x => x.password)
            .NotEmpty()
            .WithMessage("Password can't be empty")
            .MaximumLength(20)
            .WithMessage("Password must be less than 20 symbols.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters."); ;
    }
}
