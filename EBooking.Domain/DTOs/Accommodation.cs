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
    public class Accommodation
    {
        public int AccommodationId { get; set; }
        public required string Name { get; set; }
        public AccommodationType Type { get; set; }
        public required string Address { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int UserId { get; set; }
        public Administrator? Administrator { get; set; }
        public List<AccommodationUnit> AccommodationUnits { get; set; } = new();
    }
}
