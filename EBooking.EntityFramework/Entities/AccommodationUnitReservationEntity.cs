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
    [Table("AccommodationUnitReservation")]
    internal class AccommodationUnitReservationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitReservationId { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }

        [Required]
        [ForeignKey("Unit")]
        public int UnitId { get; set; }
        public AccommodationUnitEntity Unit { get; set; }

        [Required]
        public string OnName { get; set; }
        [Required]
        public DateTime ReservationFrom { get; set; }
        [Required]
        public DateTime ReservationTo { get; set; }
        [Required]
        public int NumberOfAdults { get; set; }
        [Required]
        public int NumberOfChildren { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
