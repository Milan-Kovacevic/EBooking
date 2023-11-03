using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class UnitReservation
    {
        public int UnitReservationId { get; set; }
        public required Employee Employee { get; set; }
        public required AccommodationUnit Unit { get; set; }
        public required string OnName { get; set; }
        public DateTime ReservationFrom { get; set; }
        public DateTime ReservationTo { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
