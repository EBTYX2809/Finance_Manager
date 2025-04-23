using Finance_Manager_Backend.BusinessLogic.Services;
using FluentValidation;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs.Validators;

public class GetUserSavingsQueryDTOValidator : AbstractValidator<GetUserSavingsQueryDTO>
{
    private readonly UsersService _usersService;
    public GetUserSavingsQueryDTOValidator(UsersService usersService)
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

        RuleFor(x => x.previousSavingId)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Last saving id must be greater or equal to 0.");

        RuleFor(x => x.pageSize)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(100)
            .WithMessage("Page size should to be in range from 1 to 100.");
    }
}
