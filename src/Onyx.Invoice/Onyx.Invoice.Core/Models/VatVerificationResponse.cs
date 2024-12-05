using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Models
{
    public class VatVerificationResponse
    {
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
