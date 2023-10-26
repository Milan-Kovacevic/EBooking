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
    [PrimaryKey(nameof(FeatureId), nameof(UnitId))]
    [Table("FeatureOnUnit")]
    internal class FeatureOnUnitEntity
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("UnitFeature")]
        public int FeatureId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("AccommodationUnit")]
        public int UnitId { get; set; }

        public AccommodationUnitEntity AccommodationUnit { get; set; }
        public UnitFeatureEntity UnitFeature { get; set; }
    }
}
