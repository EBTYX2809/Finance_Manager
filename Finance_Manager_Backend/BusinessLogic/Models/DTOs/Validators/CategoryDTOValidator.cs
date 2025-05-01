using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
{
    public CategoryDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name can't be empty.")
            .MaximumLength(50)
            .WithMessage("Category name can't be longer than 50 symbols.");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("Category icon can't be empty.")
            .MaximumLength(100)
            .WithMessage("Category icon can't be longer than 100 symbols.")
            .Must(icon => icon.EndsWith(".png"))
            .WithMessage("Category icon must end with '.png'.");

        RuleFor(x => x.ColorForBackground)
            .NotEmpty()
            .WithMessage("Category color for background can't be empty.")
            .MaximumLength(7)
            .WithMessage("Category color for background can't be longer than 7 symbols.")
            .Must(color => color.StartsWith("#"))
            .WithMessage("Category color for background must start with '#'.");

        RuleFor(x => x.IsIncome)
            .NotNull()
            .WithMessage("Category is income can't be empty.");

        RuleFor(x => x.ParentCategoryId)
            .Must(x => x == null || x >= 0)
            .WithMessage("Transaction inner category id can't be less than 0.");
    }
}
