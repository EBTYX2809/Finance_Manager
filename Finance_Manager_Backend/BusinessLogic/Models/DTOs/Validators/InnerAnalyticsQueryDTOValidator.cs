using Finance_Manager_Backend.BusinessLogic.Services;
using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class InnerAnalyticsQueryDTOValidator : AbstractValidator<InnerAnalyticsQueryDTO>
{
    private readonly UsersService _usersService;
    private readonly CategoriesService _categoriesService;
    public InnerAnalyticsQueryDTOValidator(UsersService usersService, CategoriesService categoriesService)
    {
        _usersService = usersService;
        _categoriesService = categoriesService;

        RuleFor(x => x.parentCategoryId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User id must be greater than 0.")
            .MustAsync(async (parentCategoryId, cancellationToken) =>
            {
                var user = await _categoriesService.GetCategoryByIdAsync(parentCategoryId);
                return true;
            });

        RuleFor(x => x.userId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User id must be greater than 0.")
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await _usersService.GetUserByIdAsync(userId);
                return true;
            });

        RuleFor(x => x.minDate)
            .NotEmpty()
            .WithMessage("Minimum date can't be empty")
            .LessThan(DateTime.UtcNow)
            .WithMessage("Minimum date can't be in future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-50))
            .WithMessage("Minimum date can't be so old in past.")
            .LessThan(x => x.maxDate)
            .WithMessage("Minimum date can't be greater than maximum date.");

        RuleFor(x => x.maxDate)
            .NotEmpty()
            .WithMessage("Maximum date can't be empty")
            .LessThan(DateTime.UtcNow)
            .WithMessage("Maximum date can't be in future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-50))
            .WithMessage("Maximum date can't be so old in past.")
            .GreaterThan(x => x.minDate)
            .WithMessage("Maximum date can't be less than minimum date.");
    }
}
