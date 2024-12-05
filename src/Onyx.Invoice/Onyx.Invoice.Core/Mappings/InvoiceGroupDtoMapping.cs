using Onyx.Invoice.Core.Entities;
using Onyx.Invoice.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Mappings
{
    public static class InvoiceGroupDtoMapping
    {
        public static InvoiceGroup ToEntity(this InvoiceGroupDto dto)
        {
            var invoiceGroup = new InvoiceGroup
            {
                IssueDate = dto.IssueDate,
                Invoices = new List<Entities.Invoice>()
            };

            foreach (var invoiceDto in dto.Invoices)
            {
                var invoice = new Entities.Invoice
                {
                    Observations = new List<Observation>()
                };

                foreach (var observationDto in invoiceDto.Observations)
                {
                    var observation = new Observation
                    {
                        TravelAgent = observationDto.TravelAgent,
                        GuestName = observationDto.GuestName,
                        NumberOfNights = observationDto.NumberOfNights
                    };

                    invoice.Observations.Add(observation);
                }

                invoiceGroup.Invoices.Add(invoice);
            }

            return invoiceGroup;
        }

        public static InvoiceDto ToDto(this Entities.Invoice invoice)
        {
            var invoiceDto = new InvoiceDto
            {
                Observations = new List<ObservationDto>()
            };

            foreach (var observation in invoice.Observations)
            {
                var observationDto = new ObservationDto
                {
                    TravelAgent = observation.TravelAgent,
                    GuestName = observation.GuestName,
                    NumberOfNights = observation.NumberOfNights
                };

                invoiceDto.Observations.Add(observationDto);
            }

            return invoiceDto;
        }

        public static ObservationDto ToDto(this Observation observation)
        {
            return new ObservationDto
            {
                TravelAgent = observation.TravelAgent,
                GuestName = observation.GuestName,
                NumberOfNights = observation.NumberOfNights
            };
        }
    }

}
