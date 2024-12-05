using Onyx.Invoice.Core.Enums;
using Onyx.Invoice.Core.Models;
using Onyx.Invoice.Core.Services;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Interfaces
{
    public interface IVatVerifier
    {
        Task<VerificationStatus> VerifyAsync(VatVerificationRequest vatRequest);
    }
}
