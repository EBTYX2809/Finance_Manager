using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class SavingTopUpDTOValidator : AbstractValidator<SavingTopUpDTO>
{
    public SavingTopUpDTOValidator()
    {
        RuleFor(x => x.savingId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Saving id must be greater than 0.");

        RuleFor(x => x.topUpAmount)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Top up amount must be greater than 0.");
    }
}
