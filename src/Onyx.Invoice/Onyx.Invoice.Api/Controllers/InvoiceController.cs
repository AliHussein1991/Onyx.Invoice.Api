using Microsoft.AspNetCore.Mvc;
using Onyx.Invoice.Core.Interfaces.Services;
using Onyx.Invoice.Core.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyx.Invoice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("addInvoiceGroup")]
        public async Task<IActionResult> AddInvoiceGroupAsync([FromBody] InvoiceGroupDto invoiceGroupDto)
        {
             await _invoiceService.AddInvoiceGroupAsync(invoiceGroupDto);
            return Ok();
        }

        [HttpPost("addTravelAgent")]
        public async Task<IActionResult> AddTravelAgentAsync([FromBody] TravelAgentInfoDto travelAgent)
        {
            await _invoiceService.AddTravelAgentAsync(travelAgent);
            return Ok();
        }

        [HttpGet("getTotalNightsByTravelAgent/{year}")]
        public async Task<IActionResult> GetTotalNightsByTravelAgentAsync(int year)
        {
            var result = await _invoiceService.GetTotalNightsByTravelAgentAsync(year);
            return Ok(result);
        }

        [HttpGet("getRepeatedGuestNames")]
        public async Task<IActionResult> GetRepeatedGuestNamesAsync()
        {
            var result = await _invoiceService.GetRepeatedGuestNamesAsync();
            return Ok(result);
        }


        [HttpGet("NoObservations")]
        public async Task<IActionResult> GetTravelAgentsWithNoObservations()
        {
            var agents = await _invoiceService.GetTravelAgentsWithNoObservationsAsync();
            return Ok(agents);
        }

        [HttpGet("MoreThanTwoObservations")]
        public async Task<IActionResult> GetTravelAgentsWithMoreThanTwoObservations()
        {
            var agents = await _invoiceService.GetTravelAgentsWithMoreThanTwoObservationsAsync();
            return Ok(agents);
        }
    }
}
