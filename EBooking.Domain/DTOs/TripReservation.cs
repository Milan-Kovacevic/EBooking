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
    public class TripReservation
    {
        public int TripId { get; set; }
        public required string OnName { get; set; }
        public TripType Type { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public required Employee Employee { get; set; }
        public List<Flight> Flights { get; set; } = new();
    }
}
