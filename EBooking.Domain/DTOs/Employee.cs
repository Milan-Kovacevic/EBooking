using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class Employee : User
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public List<UnitReservation> UnitReservations { get; set; } = new();
        public List<TripReservation> TripReservations { get; set; } = new();
    }
}
