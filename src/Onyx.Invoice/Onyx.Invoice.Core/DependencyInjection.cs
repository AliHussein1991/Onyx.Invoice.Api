using Onyx.Invoice.Core.Interfaces.Services;
using Onyx.Invoice.Core.Services;
using Onyx.Invoice.Core.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Onyx.Invoice.Core.Interfaces;
using ServiceReference1;

namespace Onyx.Invoice.Core;

public static class DependencyInjection
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddFluentValidators();
        services.AddServices();
    }

    private static void AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<InvoiceGroupDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ObservationDtoValidator>();


    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<checkVatPortTypeClient>();
        services.AddScoped<IVatVerifier, VatVerifier>();
    }
}