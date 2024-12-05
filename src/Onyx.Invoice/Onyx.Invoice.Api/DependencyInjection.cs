using Onyx.Invoice.Api.Exceptions;
using Onyx.Invoice.Core.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onyx.Invoice.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
        services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });

        services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
            .AddJsonOptions(options =>
            {                
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        services.AddEndpointsApiExplorer();

        services.AddSwagger(configuration);
        services.AddGlobalExceptionHandling();

        return services;
    }

    private static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            var openApiConfig = configuration.GetSection("OpenApi").Get<OpenApi>()!;

            options.SwaggerDoc(
                openApiConfig.Version,
                new OpenApiInfo
                {
                    Title = openApiConfig.Title,
                    Version = openApiConfig.Version,
                    Description = openApiConfig.Description
                });
        });
    }

    private static void AddGlobalExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandlerMiddleware>();
        services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
    }

    public record OpenApi(string Version, string Title, string Description);
}