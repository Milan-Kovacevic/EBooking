using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class Location
    {
        public int LocationId { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
    }
}
