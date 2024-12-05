using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Onyx.Invoice.Api.Exceptions;
using Onyx.Invoice.Core.Entities;
using Onyx.Invoice.Infrastructure.Contexts;

namespace Onyx.Invoice.Api.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<InvoiceDbContext>();
            await dbContext.Database.MigrateAsync();

            if (dbContext.InvoiceGroup.Any()) return;

            var invoiceGroups = GenerateInvoiceGroupsFor2015();
            await dbContext.InvoiceGroup.AddRangeAsync(invoiceGroups);
            await dbContext.SaveChangesAsync();
        }

        private static List<InvoiceGroup> GenerateInvoiceGroupsFor2015()
        {
            var invoiceGroups = new List<InvoiceGroup>();
            var invoiceGroup = new InvoiceGroup
            {
                IssueDate = new DateTime(2015, 6, 1),
                Invoices = GenerateInvoices()
            };
            invoiceGroups.Add(invoiceGroup);

            return invoiceGroups;
        }

        private static List<Core.Entities.Invoice> GenerateInvoices()
        {
            var invoices = new List<Core.Entities.Invoice>();
            var invoice = new Core.Entities.Invoice
            {
                Observations = GenerateObservations()
            };
            invoices.Add(invoice);
            

            return invoices;
        }

        private static List<Observation> GenerateObservations()
        {
            var observations = new List<Observation>();

            var guestNames = new List<string> { "Ali", "Manuel", "Ali" , "Manuel", "Jack"};
            foreach (var guestName in guestNames)
            {
                var observation = new Observation
                {
                    TravelAgent = "Onyx",
                    GuestName = guestName,
                    NumberOfNights = 5
                };
                observations.Add(observation);
            }

            return observations;
        }

        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
