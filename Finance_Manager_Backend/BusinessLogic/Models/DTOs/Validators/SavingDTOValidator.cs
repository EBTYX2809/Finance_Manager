using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class SavingDTOValidator : AbstractValidator<SavingDTO>
{
    public SavingDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Saving name can't be empty.")
            .MaximumLength(50)
            .WithMessage("Saving name can't be longer than 50 symbols.");

        RuleFor(x => x.Goal)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(1_000_000_000) // 10 numbers, max from db.
            .WithMessage("Saving goal slould be in range from 1 to 1,000,000,000.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Saving user id must be greater than 0.");
    }
}
