namespace EBooking.WPF.Dialogs.Models
{
    public record class LocationModel
    {
        public int LocationId { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public override string ToString() => $"{Country}, {City}";
    }
}
