using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Onyx.Invoice.Core.Enums;
using Onyx.Invoice.Core.Interfaces;
using Onyx.Invoice.Core.Models;
using ServiceReference1;

namespace Onyx.Invoice.Core.Services
{
    public class VatVerifier : IVatVerifier
    {
        private readonly IValidator<VatVerificationRequest> _validator;
        private readonly checkVatPortTypeClient _client;
        private readonly ILogger<VatVerifier> _logger;

        public VatVerifier(IValidator<VatVerificationRequest> validator, checkVatPortTypeClient client, ILogger<VatVerifier> logger)
        {
            _validator = validator;
            _client = client;
            _logger = logger;
        }

        /// <summary>
        /// Verifies the given VAT ID for the given country using the EU VIES web service.
        /// </summary>
        /// <param name="vatRequest">Request object containing countryCode and vatNumber</param>
        /// <returns>Verification status (Valid, Invalid, or Unavailable)</returns>
        public async Task<VerificationStatus> VerifyAsync(VatVerificationRequest vatRequest)
        {
            _logger.LogInformation("Starting VAT verification for CountryCode: {CountryCode}, VAT ID: {VatId}",
                vatRequest.CountryCode, vatRequest.VatId);

            try
            {
                // Validate the request
                await ValidateRequestAsync(vatRequest);

                // Call the external service
                var response = await _client.checkVatAsync(new checkVatRequest
                {
                    countryCode = vatRequest.CountryCode,
                    vatNumber = vatRequest.VatId
                });

                var status = response.valid ? VerificationStatus.Valid : VerificationStatus.Invalid;

                return status;
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation failed for VAT verification. CountryCode: {CountryCode}, VAT ID: {VatId}",
                    vatRequest.CountryCode, vatRequest.VatId);
                var errors = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during VAT verification for CountryCode: {CountryCode}, VAT ID: {VatId}",
                    vatRequest.CountryCode, vatRequest.VatId);
                return VerificationStatus.Unavailable;
            }
        }

        private async Task ValidateRequestAsync(VatVerificationRequest vatRequest)
        {
            var validationResult = await _validator.ValidateAsync(vatRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
