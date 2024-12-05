using FluentValidation;
using Onyx.Invoice.Core.Models.Dtos;
using System;

namespace Onyx.Invoice.Core.Validation
{
    public class InvoiceGroupDtoValidator : AbstractValidator<InvoiceGroupDto>
    {
        public InvoiceGroupDtoValidator()
        {
            RuleFor(x => x.IssueDate)
                .NotNull().WithMessage("Issue Date cannot be null.")
                .NotEmpty().WithMessage("Issue Date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Issue Date cannot be in the future.");

        }
    }
}
