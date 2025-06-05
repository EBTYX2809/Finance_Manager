using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Finance_Manager_Backend.Middleware;

public class ValidationFilter : IAsyncActionFilter
{
    public readonly ILogger<ValidationFilter> _logger;
    public ValidationFilter(ILogger<ValidationFilter> logger)
    {
        _logger = logger;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.Value.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator is null) continue;

            var validationContext = new ValidationContext<object>(argument.Value);
            var result = await validator.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });

                _logger.LogError("Validation failed. Errors: {errors}", errors);

                // context.Result = new BadRequestObjectResult(new
                // {
                //     Message = "Validation failed.",
                //     Errors = errors
                // });

                string errorMessage = "";
                foreach (var error in errors)
                {
                    errorMessage += $"{error.Field}: {error.Error}\n";
                }

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Message = errorMessage
                };

                context.Result = new BadRequestObjectResult(errorResponse);

                return;
            }
        }

        await next();
    }
}

