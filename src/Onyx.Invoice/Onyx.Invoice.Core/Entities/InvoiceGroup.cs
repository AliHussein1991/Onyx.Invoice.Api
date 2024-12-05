using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Entities
{
    public class InvoiceGroup
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
