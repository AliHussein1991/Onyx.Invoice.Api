using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Entities
{
    public class Invoice
    {
        public int Id { get; set; } 
        public int InvoiceGroupId { get; set; }
        public InvoiceGroup InvoiceGroup { get; set; }
        public List<Observation> Observations { get; set; }
    }
}
