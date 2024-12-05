using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Entities
{
    public class TravelAgentInfo
    {
        public int Id { get; set; }
        public string TravelAgent { get; set; }
        public int TotalNumberOfNights { get; set; }
    }
}
