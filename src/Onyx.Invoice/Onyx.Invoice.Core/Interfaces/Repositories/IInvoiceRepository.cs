using Onyx.Invoice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceGroupAsync(InvoiceGroup invoiceGroup);
        Task AddTravelAgentAsync(TravelAgentInfo travelAgent);

        Task<IEnumerable<TravelAgentInfo>> GetTotalNightsByTravelAgentAsync(int year);
        Task<IEnumerable<string>> GetRepeatedGuestNamesAsync();
        Task<IEnumerable<string>> GetTravelAgentsWithNoObservationsAsync();
        Task<IEnumerable<string>> GetTravelAgentsWithMoreThanTwoObservationsAsync();

    }
}
