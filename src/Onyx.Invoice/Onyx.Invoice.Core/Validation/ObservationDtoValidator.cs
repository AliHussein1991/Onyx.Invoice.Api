using FluentValidation;
using Onyx.Invoice.Core.Models.Dtos;

namespace Onyx.Invoice.Core.Validation
{
    public class ObservationDtoValidator : AbstractValidator<ObservationDto>
    {
        public ObservationDtoValidator()
        {
            RuleFor(x => x.GuestName)
                .NotNull().WithMessage("Guest Name cannot be null.")
                .NotEmpty().WithMessage("Guest Name is required.")
                .MaximumLength(100).WithMessage("Guest Name cannot exceed 100 characters.");

            RuleFor(x => x.TravelAgent)
                .NotNull().WithMessage("Travel Agent cannot be null.")
                .NotEmpty().WithMessage("Travel Agent is required.")
                .MaximumLength(50).WithMessage("Travel Agent cannot exceed 50 characters.");

            RuleFor(x => x.NumberOfNights)
                .GreaterThan(0).WithMessage("Number of Nights must be greater than 0.");
        }
    }
}
