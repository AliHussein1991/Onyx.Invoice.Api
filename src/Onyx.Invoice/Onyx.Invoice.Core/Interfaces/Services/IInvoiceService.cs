using Onyx.Invoice.Core.Entities;
using Onyx.Invoice.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task AddInvoiceGroupAsync(InvoiceGroupDto invoiceGroup);
        Task AddTravelAgentAsync(TravelAgentInfoDto travelAgent);

        Task<IEnumerable<TravelAgentInfoDto>> GetTotalNightsByTravelAgentAsync(int year);
        Task<IEnumerable<string>> GetRepeatedGuestNamesAsync();
        Task<IEnumerable<string>> GetTravelAgentsWithNoObservationsAsync();
        Task<IEnumerable<string>> GetTravelAgentsWithMoreThanTwoObservationsAsync();
    }
}
