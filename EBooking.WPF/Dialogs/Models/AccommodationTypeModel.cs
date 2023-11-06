using EBooking.Domain.Enums;

namespace EBooking.WPF.Dialogs.Models
{
    public record class AccommodationTypeModel(AccommodationType Type)
    {
        public string TypeName { get => Type.ToString().ToLowerInvariant(); }
        public override string ToString() => $"{TypeName}";
    }
}
