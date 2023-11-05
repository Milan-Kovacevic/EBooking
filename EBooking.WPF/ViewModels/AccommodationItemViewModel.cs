using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.Domain.DTOs;
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
        private int accommodationId;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string typeText;
        private AccommodationType? _type;
        public AccommodationType? Type
        {
            get => _type;
            set
            {
                _type = value;
                if (value is not null)
                {
                    TypeText = $"{value.ToString()?.ToLowerInvariant()}";
                    SetIsApartment(value);
                }   
            }
        }

        [ObservableProperty]
        private string locationText;
        private Location? _location;
        public Location? Location
        {
            get => _location;
            set
            {
                _location = value;
                if (value is not null)
                    LocationText = $"{value.Country}, {value.City}";
            }
        }
        [ObservableProperty]
        private string address;
        [ObservableProperty]
        private int numOfUnits;
        [ObservableProperty]
        private bool isApartment;

        public AccommodationItemViewModel()
        {
            name = string.Empty;
            typeText = string.Empty;
            _type = null;
            locationText = string.Empty;
            _location = null;
            address = string.Empty;
            numOfUnits = 0;
            isApartment = false;
        }

        private void SetIsApartment(AccommodationType? value)
        {
            if (value is null)
                IsApartment = false;
            else if (value == AccommodationType.APARTMENT)
                IsApartment = true;
            else if (value == AccommodationType.HOTEL)
                IsApartment = false;
            else
                IsApartment = false;
        }
    }
}
