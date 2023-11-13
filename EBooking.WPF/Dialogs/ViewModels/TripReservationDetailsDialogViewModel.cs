using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Dialogs.Models;
using EBooking.WPF.Services;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class TripReservationDetailsDialogViewModel : ObservableObject, IViewModelBase
    {
        public string OnName { get; set; }
        public string TripType { get; set; }
        public string NumberOfSeats { get; set; }
        public string ReservedBy { get; set; }
        public string TotalPrice { get; set; }

        private readonly List<FlightModel> _flights;
        public ListCollectionView AddedFlightsCollection { get; }
        [ObservableProperty]
        private int selectedAddedFlightIndex;

        public TripReservationDetailsDialogViewModel(TripReservationService tripReservationService)
        {
            var tripReservation = tripReservationService.GetSelectedTripReservation();
            _flights = new List<FlightModel>(tripReservation?.Flights.Select(x => Mapper.Map(x).ToANew<FlightModel>()) ?? Array.Empty<FlightModel>());
            AddedFlightsCollection = new ListCollectionView(_flights);

            OnName = tripReservation?.OnName ?? string.Empty;
            NumberOfSeats = tripReservation?.NumberOfSeats.ToString() ?? string.Empty;
            TripType = tripReservation?.Type.ToString().ToLower().Replace('_', ' ') ?? string.Empty;
            TotalPrice = tripReservation?.TotalPrice.ToString() ?? "0.0";
            ReservedBy = $"{tripReservation?.Employee?.FirstName} {tripReservation?.Employee?.LastName}";
            selectedAddedFlightIndex = -1;
        }

        [RelayCommand]
        public void RemoveSelectedFlight()
        {

        }
    }
}
