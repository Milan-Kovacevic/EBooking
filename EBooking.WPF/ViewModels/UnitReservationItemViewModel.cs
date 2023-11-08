using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class UnitReservationItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private bool isOwner;
        [ObservableProperty]
        private int unitReservationId;
        [ObservableProperty]
        private string onName;
        [ObservableProperty]
        private string reservationPeriod;
        [ObservableProperty]
        private string numberOfPeople;
        [ObservableProperty]
        private decimal totalPrice;

        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public DateTime? ReservationFrom { get; set; }
        public DateTime? ReservationTo { get; set; }

        public UnitReservationItemViewModel()
        {
            isSelected = false;
            isOwner = false;
            unitReservationId = 0;
            onName = string.Empty;
            reservationPeriod = string.Empty;
            numberOfPeople = string.Empty;
            NumberOfAdults = 0;
            NumberOfChildren = 0;
            ReservationFrom = null;
            ReservationTo = null;
        }
    }
}
