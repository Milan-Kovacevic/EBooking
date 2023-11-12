using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ItemViewModels
{
    public partial class TripReservationItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private bool isOwner;
        [ObservableProperty]
        private int tripReservationId;
        [ObservableProperty]
        private string onName;
        [ObservableProperty]
        private string tripType;
        [ObservableProperty]
        private int numberOfSeats;
        [ObservableProperty]
        private decimal totalPrice;
        [ObservableProperty]
        private string reservedBy;
        [ObservableProperty]
        private string tripSummary;

        public TripReservationItemViewModel()
        {
            isSelected = false;
            isOwner = false;
            tripReservationId = 0;
            onName = string.Empty;
            tripType = string.Empty;
            numberOfSeats = 0;
            totalPrice = 0.0m;
            reservedBy = string.Empty;
            tripSummary = string.Empty;
        }
    }
}
