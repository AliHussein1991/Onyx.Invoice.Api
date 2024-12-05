using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Entities
{
    public class Observation
    {
        public int Id { get; set; } 
        public int InvoiceId { get; set; } 
        public Invoice Invoice { get; set; }
        public string TravelAgent { get; set; }
        public string GuestName { get; set; }
        public int NumberOfNights { get; set; }
    }
}
