using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Models.Dtos
{
    public class InvoiceDto
    {
        public List<ObservationDto> Observations { get; set; }
    }
}
