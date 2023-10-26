using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.Entities
{
    [Table("AccommodationUnit")]
    internal class AccommodationUnitEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime AvailableFrom { get; set; }
        [Required]
        public DateTime AvailableTo { get; set; }
        [Required]
        public int NumberOfBeds { get; set; }
        [Required]
        public decimal PricePerNight { get; set; }
        public decimal? UnitSize { get; set; }

        [Required]
        [ForeignKey("Accommodation")]
        public int AccommodationId { get; set; }
        public AccommodationEntity Accommodation { get; set; }
        public List<FeatureOnUnitEntity>  Features { get; set; }
    }
}
