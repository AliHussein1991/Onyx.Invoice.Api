using Onyx.Invoice.Core.Entities;
using Onyx.Invoice.Core.Interfaces.Repositories;
using Onyx.Invoice.Core.Interfaces.Services;
using Onyx.Invoice.Core.Mappings;
using Onyx.Invoice.Core.Models.Dtos;

namespace Onyx.Invoice.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task AddInvoiceGroupAsync(InvoiceGroupDto invoiceGroupDto)
        {
            var invoiceGroup = invoiceGroupDto.ToEntity();
            await _invoiceRepository.AddInvoiceGroupAsync(invoiceGroup);
        }

        public async Task AddTravelAgentAsync(TravelAgentInfoDto travelAgentDto)
        {
            var travelAgent = travelAgentDto.ToEntity();
            await _invoiceRepository.AddTravelAgentAsync(travelAgent);
        }

        public async Task<IEnumerable<TravelAgentInfoDto>> GetTotalNightsByTravelAgentAsync(int year)
        {
            var travelAgents = await _invoiceRepository.GetTotalNightsByTravelAgentAsync(year);
            return travelAgents.Select(ta => ta.ToDto());
        }

        public async Task<IEnumerable<string>> GetRepeatedGuestNamesAsync()
        {
            return await _invoiceRepository.GetRepeatedGuestNamesAsync();
        }

        public async Task<IEnumerable<string>> GetTravelAgentsWithNoObservationsAsync()
        {
            return await _invoiceRepository.GetTravelAgentsWithNoObservationsAsync();
        }

        public async Task<IEnumerable<string>> GetTravelAgentsWithMoreThanTwoObservationsAsync()
        {
            return await _invoiceRepository.GetTravelAgentsWithMoreThanTwoObservationsAsync();
        }
    }
}
