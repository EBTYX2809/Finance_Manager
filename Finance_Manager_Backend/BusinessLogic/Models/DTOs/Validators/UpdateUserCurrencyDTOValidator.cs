using Finance_Manager_Backend.BusinessLogic.Services;
using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class UpdateUserCurrencyDTOValidator : AbstractValidator<UpdateUserCurrencyQueryDTO>
{
    private UsersService _usersService;
    public UpdateUserCurrencyDTOValidator(UsersService usersService)
    {
        _usersService = usersService;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User id must be greater than 0.")
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await _usersService.GetUserByIdAsync(userId);
                return true;
            });

        RuleFor(x => x.CurrencyRang)
            .NotEmpty()
            .WithMessage("CurrencyRang can't be empty")
            .MinimumLength(7)
            .WithMessage("CurrencyRang must be minimum 7 symbols.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .WithMessage("CurrencyRang can't be empty")
            .Length(3)
            .WithMessage("CurrencyCode must be exactly 3 characters long.");
    }
}
