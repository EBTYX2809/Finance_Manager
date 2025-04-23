using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class TransactionDTOValidator : AbstractValidator<TransactionDTO>
{
    public TransactionDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Transaction name can't be empty.")
            .MaximumLength(50)
            .WithMessage("Transaction name can't be longer than 50 symbols.");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(1_000_000_000) // 10 numbers, max from db.
            .WithMessage("Price must be in range from 1 to 1,000,000,000.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Transaction date can't be empty")
            .LessThan(DateTime.UtcNow)
            .WithMessage("Transaction date can't be in future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-50))
            .WithMessage("Transaction date can't be so old in past.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Transaction user id must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Transaction category id must be greater than 0.");

        RuleFor(x => x.InnerCategoryId)
            .Must(x => x == null || x >= 0)
            .WithMessage("Transaction inner category id can't be less than 0.");
    }
}
