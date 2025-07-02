using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using MiniCommerce.Api.App.Models;

namespace MiniCommerce.Api.App.Middlewares;

[AutoConstructor]
public partial class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);

            if (httpContext.Response.StatusCode >= 400 && !httpContext.Response.HasStarted)
            {
                await HandleFailedAsync(httpContext);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleFailedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var result = new ErrorModel()
        {
            Status = context.Response.StatusCode,
            Message = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode),
            Detail = "An error occurred while processing your request."
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorResponse = new ErrorModel()
        {
            Status = context.Response.StatusCode,
            Message = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred.",
            Detail = _env.IsDevelopment() ? exception.StackTrace : null
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}