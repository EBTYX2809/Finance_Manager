using Finance_Manager_Backend.BusinessLogic.Services;
using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class UserIdTelegramIdDTOValidator : AbstractValidator<UserIdTelegramIdDTO>
{
    private UsersService _usersService;
    public UserIdTelegramIdDTOValidator(UsersService usersService)
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

        RuleFor(x => x.TelegramId)
            .NotEmpty()
            .WithMessage("TelegramId can't be empty");
    }
}
