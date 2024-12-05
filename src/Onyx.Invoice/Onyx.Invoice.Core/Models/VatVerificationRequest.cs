using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Models
{
    public class VatVerificationRequest
    {
        public string CountryCode { get; set; } = null!;
        public string VatId { get; set; } = null!;
    }
}
