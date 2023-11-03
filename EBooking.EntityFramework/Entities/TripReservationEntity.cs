using EBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.Entities
{
    [Table("TripReservation")]
    internal class TripReservationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TripId { get; set; }
        [Required]
        public string OnName { get; set; }
        [Required]
        public TripType Type { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }
        public List<FlightOnTripReservationEntity> Flights { get; set; }
    }
}
