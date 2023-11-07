using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.Models
{
    public record class UnitFeatureModel
    {
        public int FeatureId { get; set; }
        public required string Name { get; set; }

        public override string ToString() => $"{Name}";
    }
}
