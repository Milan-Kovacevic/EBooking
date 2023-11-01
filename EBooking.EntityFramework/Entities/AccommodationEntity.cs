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
    [Table("Accommodation")]
    internal class AccommodationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccommodationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public AccommodationType Type { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public LocationEntity Location { get; set; }

        [Required]
        [ForeignKey("Administrator")]
        public int UserId { get; set; }
        public AdministratorEntity Administrator { get; set; }
        public List<AccommodationUnitEntity> AccommodationUnits { get; set; }

    }
}
