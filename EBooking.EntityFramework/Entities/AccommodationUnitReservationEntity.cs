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
    [PrimaryKey(nameof(EmployeeId), nameof(UnitId))]
    [Table("AccommodationUnitReservation")]
    internal class AccommodationUnitReservationEntity
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("Unit")]
        public int UnitId { get; set; }
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

        public EmployeeEntity Employee { get; set; }
        public AccommodationUnitEntity Unit { get; set; }
    }
}
