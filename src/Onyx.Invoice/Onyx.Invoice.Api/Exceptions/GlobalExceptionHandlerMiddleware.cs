using Onyx.Invoice.Core.Results;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onyx.Invoice.Api.Exceptions;

public class GlobalExceptionHandlerMiddleware() : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var result = new Result<string>(ex.Message);

            await HandleExceptionAsync(context, result);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Result<string> result)
    {
        const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(result, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        return context.Response.WriteAsync(response);
    }
}