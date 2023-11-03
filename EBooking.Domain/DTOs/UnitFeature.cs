using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class UnitFeature
    {
        public int FeatureId { get; set; }
        public required string Name { get; set; }
    }
}
