using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Onyx.Invoice.Core.Enums;
using Onyx.Invoice.Core.Interfaces;
using Onyx.Invoice.Core.Models;
using Onyx.Invoice.Core.Services;
using System.Threading.Tasks;

namespace Onyx.Invoice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatVerificationController : ControllerBase
    {
        private readonly IVatVerifier _vatVerifier;

        // Dependency injection of the IVatVerifier service
        public VatVerificationController(IVatVerifier vatVerifier)
        {
            _vatVerifier = vatVerifier;
        }

        /// <summary>
        /// Verifies a VAT ID.
        /// </summary>
        /// <param name="request">VAT verification request containing country code and VAT ID.</param>
        /// <returns>Verification result.</returns>
        [HttpPost]
        public async Task<IActionResult> VerifyVatId([FromBody] VatVerificationRequest request)
        {
            try
            {
                var result = await _vatVerifier.VerifyAsync(request);

                return result switch
                {
                    VerificationStatus.Valid => Ok(new VatVerificationResponse { Status = VerificationStatus.Valid.ToString(), Message = "The VAT ID is valid." }),
                    VerificationStatus.Invalid => Ok(new VatVerificationResponse { Status = VerificationStatus.Invalid.ToString(), Message = "The VAT ID is invalid." }),
                    VerificationStatus.Unavailable => StatusCode(503, new VatVerificationResponse { Status = VerificationStatus.Unavailable.ToString(), Message = "The service is unavailable. Please try again later." }),
                    _ => StatusCode(500, new { Status = "Error", Message = "An unknown error occurred." })
                };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
