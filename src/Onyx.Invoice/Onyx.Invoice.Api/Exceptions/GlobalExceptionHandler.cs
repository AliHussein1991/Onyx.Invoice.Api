using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Onyx.Invoice.Api.Exceptions;

public class GlobalExceptionHandler(IHostEnvironment environment) : IExceptionHandler
{
    private const bool IsLastStopInPipeline = true;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

  

        var (statusCode, title) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Extensions = { ["traceId"] = traceId },
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        if (!environment.IsProduction()) problemDetails.Detail = exception.Message;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return IsLastStopInPipeline;
    }

    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            TaskCanceledException _ => (StatusCodes.Status504GatewayTimeout, "Request Timeout"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }
}