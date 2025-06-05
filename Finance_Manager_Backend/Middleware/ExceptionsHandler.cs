using Finance_Manager_Backend.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace Finance_Manager_Backend.Middleware;

public class ExceptionsHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionsHandler> _logger;

    public ExceptionsHandler(RequestDelegate next, ILogger<ExceptionsHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        var response = httpContext.Response;
        var errorDetails = new ErrorResponse();

        switch (exception)
        {
            case EntityNotFoundException entityIsNotExist:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = entityIsNotExist.Message
                };
                break;

            case InvalidOperationException invalidOperation:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = invalidOperation.Message
                };
                break;

            case SecurityTokenException tokenException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = tokenException.Message
                };
                break;

            case UnauthorizedAccessException unauthorizedAccess:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = unauthorizedAccess.Message
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogCritical(exception, "Unexpected error occurred: {Message}", exception.Message);

                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Unknown error."
                };
                break;
        }

        _logger.LogError(exception, exception.Message);

        var result = JsonSerializer.Serialize(errorDetails);
        return httpContext.Response.WriteAsync(result);
    }

}
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
