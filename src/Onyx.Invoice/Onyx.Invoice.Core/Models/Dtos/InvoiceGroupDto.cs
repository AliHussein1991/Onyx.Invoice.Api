using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Models.Dtos
{
    public class InvoiceGroupDto
    {
        public DateTime IssueDate { get; set; }
        public List<InvoiceDto> Invoices { get; set; }
    }
}
