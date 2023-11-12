using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.Models
{
    public record class FlightModel
    {
        public int FlightId { get; set; }
        public FlightClass FlightClass { get; set; }
        public required string AvioCompanyName { get; set; }
        public decimal TicketPrice { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureTimeText { get => $"{DepartureTime.ToLongDateString()} {DepartureTime.ToShortTimeString()}"; }
        public DateTime ArrivalTime { get; set; }
        public string ArrivalTimeText { get => $"{ArrivalTime.ToLongDateString()} {ArrivalTime.ToShortTimeString()}"; }

        public int UserId { get; set; }
        public LocationModel? FromLocation { get; set; }
        public LocationModel? ToLocation { get; set; }


        public override string ToString()
        {
            return $"*{AvioCompanyName}* {FromLocation} - {ToLocation}";
        }
    }
}
