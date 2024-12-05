using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onyx.Invoice.Core.Entities;
using Onyx.Invoice.Core.Interfaces.Repositories;
using Onyx.Invoice.Infrastructure.Contexts;
using Onyx.Invoice.Infrastructure.Queries;

namespace Onyx.Invoice.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceDbContext _dbContext;
        private readonly ILogger<InvoiceRepository> _logger;

        public InvoiceRepository(InvoiceDbContext dbContext, ILogger<InvoiceRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new InvoiceGroup with its associated Invoices and Observations to the database.
        /// </summary>
        /// <param name="invoiceGroup">The InvoiceGroup to be added</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task AddInvoiceGroupAsync(InvoiceGroup invoiceGroup)
        {
            await _dbContext.InvoiceGroup.AddAsync(invoiceGroup);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a new TravelAgent to the database.
        /// </summary>
        /// <param name="travelAgent">The TravelAgent to be added</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task AddTravelAgentAsync(TravelAgentInfo travelAgent)
        {
            await _dbContext.TravelAgent.AddAsync(travelAgent);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves the total number of nights for each travel agent for invoices issued in a specific year.
        /// </summary>
        /// <param name="year">The year to filter the invoice groups</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of TravelAgentInfo</returns>
        public async Task<IEnumerable<TravelAgentInfo>> GetTotalNightsByTravelAgentAsync(int year)
        {
            IEnumerable<TravelAgentInfo> numberOfNightsByTravelAgent  =  await _dbContext.InvoiceGroup
                .Where(ig => ig.IssueDate.Year == year)
                .SelectMany(ig => ig.Invoices)
                .SelectMany(i => i.Observations)
                .GroupBy(o => o.TravelAgent)
                .Select(g => new TravelAgentInfo
                {
                    TravelAgent = g.Key,
                    TotalNumberOfNights = g.Sum(o => o.NumberOfNights)
                })
                .ToListAsync();
            return numberOfNightsByTravelAgent;
        }

        /// <summary>
        /// Retrieves the guest names that appear more than once across all invoices and invoice groups.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a list of guest names</returns>
        public async Task<IEnumerable<string>> GetRepeatedGuestNamesAsync()
        {
            IEnumerable<string> repeatedGuestNames = await _dbContext.InvoiceGroup
                .SelectMany(ig => ig.Invoices)
                .SelectMany(i => i.Observations)
                .GroupBy(o => o.GuestName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToListAsync();

            return repeatedGuestNames;
        }

        public async Task<IEnumerable<string>> GetTravelAgentsWithNoObservationsAsync()
        {
            return await ExecuteRawSqlQueryAsync(SqlQueries.TravelAgentsWithNoObservations);
        }

        public async Task<IEnumerable<string>> GetTravelAgentsWithMoreThanTwoObservationsAsync()
        {
            return await ExecuteRawSqlQueryAsync(SqlQueries.TravelAgentsWithMoreThanTwoObservations);
        }

        private async Task<IEnumerable<string>> ExecuteRawSqlQueryAsync(string query)
        {
            return await _dbContext.TravelAgent
                .FromSqlRaw(query)
                .Select(t => t.TravelAgent)
                .ToListAsync();
        }

    }
}
