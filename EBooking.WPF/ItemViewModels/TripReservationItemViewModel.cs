using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Dialogs.Models;
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
        private string tripTypeText;
        private TripType _tripType;

        public TripType Type
        {
            get => _tripType;
            set
            {
                _tripType = value;
                TripTypeText = $"{value.ToString().ToLower().Replace('_', ' ')}";
            }
        }

        [ObservableProperty]
        private int numberOfSeats;
        [ObservableProperty]
        private decimal totalPrice;
        [ObservableProperty]
        private string reservedBy;
        [ObservableProperty]
        private string tripSummary;
        public List<FlightModel> Flights { get; set; } = new();

        public TripReservationItemViewModel(TripReservation tripReservation, User? user)
        {
            tripReservationId = 0;
            onName = string.Empty;
            tripTypeText = string.Empty;
            _tripType = TripType.ONE_WAY;
            numberOfSeats = 0;
            totalPrice = 0.0m;
            tripSummary = string.Empty;
            isSelected = false;
            isOwner = user?.UserId == tripReservation.EmployeeId;
            reservedBy = $"{tripReservation.Employee?.FirstName} {tripReservation.Employee?.LastName}";
            Mapper.Map(tripReservation).Over(this);
            SetTripSummary();
        }

        public void SetTripSummary()
        {
            if (Flights.Count == 0)
                return;
            string takeOff = $"{Flights[0].FromLocation}";
            string landing = $"{Flights[0].ToLocation}";
            if (Flights.Count >= 2)
                landing = $"{Flights[Flights.Count - 1].ToLocation}";
            TripSummary = $"{takeOff} - {landing}";
        }
    }
}
