using ProjectDataManager.Contracts.Exceptions;
using System.Text.Json;

namespace ProjectDataManager.Middleware;

public class ApiExceptionsMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger _logger;

    public ApiExceptionsMiddleware(RequestDelegate next, ILogger<ApiExceptionsMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {Method} {Path} failed.", context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, exception.Message),
            ForbiddenException => (StatusCodes.Status403Forbidden, exception.Message),
            InvalidStateException => (StatusCodes.Status409Conflict, exception.Message),
            InsufficientComponentsException => (StatusCodes.Status400BadRequest, exception.Message),
            UpdateStateException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            RegistrationFailedException => (StatusCodes.Status409Conflict, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error.")
        };

        var isDevelopment = context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        var response = new
        {
            error = message,
            stackTrace = isDevelopment ? exception.StackTrace : null
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
