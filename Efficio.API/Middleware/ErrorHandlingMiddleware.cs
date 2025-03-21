using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Efficio.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = HttpStatusCode.InternalServerError; // 500 by default
        var errorMessage = "An unexpected error occurred.";
        
        // Customize based on exception type
        if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            errorMessage = "The requested resource was not found.";
        }
        else if (exception is UnauthorizedAccessException)
        {
            statusCode = HttpStatusCode.Unauthorized;
            errorMessage = "You are not authorized to access this resource.";
        }
        // Add more exception types as needed
        
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = statusCode.ToString(),
            message = errorMessage,
            // Don't include detailed exception info in production
            // detail = exception.Message 
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}