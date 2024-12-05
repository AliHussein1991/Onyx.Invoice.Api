using FluentValidation;
using Onyx.Invoice.Core.Models;
using Onyx.Invoice.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Validation
{
    public class VatVerificationRequestValidator : AbstractValidator<VatVerificationRequest>
    {
        public VatVerificationRequestValidator()
        {
            RuleFor(request => request.CountryCode)
              .NotEmpty().WithMessage("Country code is required.")
              .Length(2).WithMessage("Country code must be exactly 2 characters.")
              .Matches("^[A-Z]{2}$").WithMessage("Country code must contain only uppercase letters.");

            RuleFor(request => request.VatId)
                .NotEmpty().WithMessage("VAT ID is required.")
                .Matches("^[0-9]+$").WithMessage("VAT ID must contain only numeric characters.")
                .Length(5, 15).WithMessage("VAT ID must be between 5 and 15 characters long.");
        }
    }
    
}
