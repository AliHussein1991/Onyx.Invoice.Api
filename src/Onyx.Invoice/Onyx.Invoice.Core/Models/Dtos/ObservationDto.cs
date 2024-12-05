using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Models.Dtos
{
    public class ObservationDto
    {
        public string TravelAgent { get; set; }
        public string GuestName { get; set; }
        public int NumberOfNights { get; set; }
    }
}
