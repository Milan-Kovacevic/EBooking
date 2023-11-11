using EBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class Flight
    {
        public int FlightId { get; set; }
        public FlightClass FlightClass { get; set; }
        public required string AvioCompanyName { get; set; }
        public decimal TicketPrice { get; set; }
        public int FlightCapacity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public int UserId { get; set; }
        public Administrator? Administrator { get; set; }
        public int FromLocationId { get; set; }
        public Location? FromLocation { get; set; }
        public int ToLocationId { get; set; }
        public Location? ToLocation { get; set; }
    }
}
