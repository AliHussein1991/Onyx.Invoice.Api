using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Utils.Logger
{
    public class DefaultTimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime() => DateTime.Now;
    }
}
