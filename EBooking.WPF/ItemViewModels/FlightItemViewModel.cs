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
    public partial class FlightItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isOwner;
        [ObservableProperty]
        private int flightId;
        [ObservableProperty]
        private string avioCompanyName;
        [ObservableProperty]
        private string flightClassText;
        private FlightClass? _flightClass;
        public FlightClass? FlightClass
        {
            get => _flightClass;
            set
            {
                _flightClass = value;
                if (value is not null)
                {
                    FlightClassText = $"{value.ToString()?.ToLowerInvariant().Replace('_', ' ')}";
                }
            }
        }
        [ObservableProperty]
        private decimal ticketPrice;

        [ObservableProperty]
        private string takeOffDateText;
        [ObservableProperty]
        private string takeOffTimeText;
        private DateTime _departureTime;
        public DateTime DepartureTime
        {
            get => _departureTime;
            set
            {
                _departureTime = value;
                TakeOffDateText = $"{value.ToLongDateString()}";
                TakeOffTimeText = $"{value.ToShortTimeString()}";
            }
        }

        [ObservableProperty]
        private string landingDateText;
        [ObservableProperty]
        private string landingTimeText;
        private DateTime _arrivalTime;
        public DateTime ArrivalTime
        {
            get => _arrivalTime;
            set
            {
                _arrivalTime = value;
                LandingDateText = $"{value.ToLongDateString()}";
                LandingTimeText = $"{value.ToShortTimeString()}";
            }
        }

        [ObservableProperty]
        private string fromLocationText;
        private Location? _fromLocation;
        public Location? FromLocation
        {
            get => _fromLocation;
            set
            {
                _fromLocation = value;
                if (value is not null)
                    FromLocationText = $"{value.Country}, {value.City}";
            }
        }

        [ObservableProperty]
        private string toLocationText;
        private Location? _toLocation;
        public Location? ToLocation
        {
            get => _toLocation;
            set
            {
                _toLocation = value;
                if (value is not null)
                    ToLocationText = $"{value.Country}, {value.City}";
            }
        }

        private readonly FlightService _flightService;
        private readonly DialogHostService _dialogHostService;

        public FlightItemViewModel(FlightService flightService, DialogHostService dialogHostService)
        {
            _flightService = flightService;
            _dialogHostService = dialogHostService;

            isOwner = false;
            avioCompanyName = string.Empty;
            flightId = 0;
            flightClassText = string.Empty;
            ticketPrice = 0.0m;
            takeOffDateText = string.Empty;
            takeOffTimeText = string.Empty;
            _departureTime = DateTime.Now;
            landingDateText = string.Empty;
            landingTimeText = string.Empty;
            _arrivalTime = DateTime.Now;
            fromLocationText = string.Empty;
            _fromLocation = null;
            toLocationText = string.Empty;
            _toLocation = null;

        }

        [RelayCommand]
        public void DeleteFlight()
        {
            _flightService.SetSelectedFlight(FlightId);
            _dialogHostService.OpenFightDeleteDialog();
        }

        [RelayCommand]
        public void EditFlight()
        {
            _flightService.SetSelectedFlight(FlightId);
            _dialogHostService.OpenFlightEditDialog();
        }
    }
}
