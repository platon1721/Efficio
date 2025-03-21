using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Efficio.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // Log request information
        _logger.LogInformation(
            "Request {Method} {Path} started at {Time}",
            context.Request.Method,
            context.Request.Path,
            DateTime.UtcNow);

        try
        {
            await _next(context);
            stopwatch.Stop();

            // Log successful response
            _logger.LogInformation(
                "Request {Method} {Path} completed with status code {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception)
        {
            stopwatch.Stop();
            
            // Log exception (the actual exception will be handled by ErrorHandlingMiddleware)
            _logger.LogWarning(
                "Request {Method} {Path} failed in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
                
            throw; // Re-throw to let ErrorHandlingMiddleware handle it
        }
    }
}