using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ItemViewModels
{
    public partial class AccommodationItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isOwner;
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

        private readonly AccommodationService _accommodationService;
        private readonly DialogHostService _dialogHostService;
        private readonly NavigationService _navigateToAccommodationDetailsViewModel;

        public AccommodationItemViewModel(AccommodationService accommodationService, DialogHostService dialogHostService, NavigationService navigateToAccommodationDetailsViewModel)
        {
            isOwner = false;
            name = string.Empty;
            typeText = string.Empty;
            _type = null;
            locationText = string.Empty;
            _location = null;
            address = string.Empty;
            numOfUnits = 0;
            isApartment = false;
            _accommodationService = accommodationService;
            _dialogHostService = dialogHostService;
            _navigateToAccommodationDetailsViewModel = navigateToAccommodationDetailsViewModel;
        }

        private void SetIsApartment(AccommodationType? value)
        {
            if (value is not null && value == AccommodationType.APARTMENT)
                IsApartment = true;
            else
                IsApartment = false;
        }

        [RelayCommand]
        public void DeleteAccommodation()
        {
            _accommodationService.SetSelectedAccommodation(AccommodationId);
            _dialogHostService.OpenAccommodationDeleteDialog();
        }

        [RelayCommand]
        public void EditAccommodation()
        {
            _accommodationService.SetSelectedAccommodation(AccommodationId);
            _dialogHostService.OpenAccommodationEditDialog();
        }

        public void ShowAccommodationDetails()
        {
            _accommodationService.SetSelectedAccommodation(AccommodationId);
            _navigateToAccommodationDetailsViewModel.Navigate();
        }
    }
}
