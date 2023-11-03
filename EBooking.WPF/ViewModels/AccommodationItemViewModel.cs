using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class AccommodationItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string type;
        partial void OnTypeChanged(string value)
        {
            SetIsApartment(value);
        }
        [ObservableProperty]
        private string location;
        [ObservableProperty]
        private string address;
        [ObservableProperty]
        private int numOfUnits;
        [ObservableProperty]
        private bool isApartment;

        public AccommodationItemViewModel()
        {
            name = string.Empty;
            type = string.Empty;
            location = string.Empty;
            address = string.Empty;
            numOfUnits = 0;
            SetIsApartment(type);
        }

        private void SetIsApartment(string value)
        {
            if (value.ToUpper() == AccommodationType.APARTMENT.ToString())
                IsApartment = true;
            else if (value.ToUpper() == AccommodationType.HOTEL.ToString())
                IsApartment = false;
            else
                IsApartment = false;
        }
    }
}
