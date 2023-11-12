using EBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.Models
{
    public record class TripTypeModel(TripType Type)
    {
        public string TypeName { get => Type.ToString().ToLowerInvariant().Replace('_', ' '); }
        public override string ToString() => $"{TypeName}";
    }
}
