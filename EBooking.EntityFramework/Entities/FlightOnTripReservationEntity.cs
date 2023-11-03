using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.Entities
{
    [PrimaryKey(nameof(TripId), nameof(FlightId))]
    [Table("FlightOnTripReservation")]
    internal class FlightOnTripReservationEntity
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("TripReservation")]
        public int TripId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("Flight")]
        public int FlightId { get; set; }

        public TripReservationEntity TripReservation { get; set; }
        public FlightEntity Flight { get; set; }
    }
}
