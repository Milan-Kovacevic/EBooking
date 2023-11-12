using EBooking.Domain.DTOs;
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
    [Table("Flight")]
    internal class FlightEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightId { get; set; }
        [Required]
        public FlightClass FlightClass { get; set; }
        [Required]
        public string AvioCompanyName { get; set; }
        [Required]
        public decimal TicketPrice { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [ForeignKey("Administrator")]
        public int UserId { get; set; }
        public AdministratorEntity Administrator { get; set; }

        [Required]
        [ForeignKey("FromLocation")]
        public int FromLocationId { get; set; }
        public LocationEntity FromLocation { get; set; }

        [Required]
        [ForeignKey("ToLocation")]
        public int ToLocationId { get; set; }
        public LocationEntity ToLocation { get; set; }
    }
}
