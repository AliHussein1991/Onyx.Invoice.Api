using Onyx.Invoice.Core.Interfaces.Repositories;
using Onyx.Invoice.Infrastructure.Contexts;
using Onyx.Invoice.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Onyx.Invoice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        services.AddDbContext<InvoiceDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure(
                    5,
                    TimeSpan.FromSeconds(10),
                    null);
            });
        });

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        return services;
    }
}