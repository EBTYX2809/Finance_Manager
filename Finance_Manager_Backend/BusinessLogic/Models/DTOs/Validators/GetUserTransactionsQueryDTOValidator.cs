using Finance_Manager_Backend.BusinessLogic.Services;
using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class GetUserTransactionsQueryDTOValidator : AbstractValidator<GetUserTransactionsQueryDTO>
{
    private readonly UsersService _usersService;
    public GetUserTransactionsQueryDTOValidator(UsersService usersService)
    {
        _usersService = usersService;

        RuleFor(x => x.userId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User id must be greater than 0.")
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await _usersService.GetUserByIdAsync(userId);
                return true;
            });

        When(x => x.lastDate.HasValue, () =>
        {
            RuleFor(x => x.lastDate.Value)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Last date can't be in the future.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-50))
                .WithMessage("Last date can't be older than 50 years.");
        });

        RuleFor(x => x.pageSize)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(100)
            .WithMessage("Page size should to be in range from 1 to 100.");
    }
}
