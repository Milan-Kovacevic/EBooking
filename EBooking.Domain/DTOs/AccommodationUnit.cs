using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class AccommodationUnit
    {
        public int UnitId { get; set; }
        public required string Name { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public int NumberOfBeds { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal? UnitSize { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation? Accommodation { get; set; }
        public List<UnitFeature> Features { get; set; } = new();
    }
}
