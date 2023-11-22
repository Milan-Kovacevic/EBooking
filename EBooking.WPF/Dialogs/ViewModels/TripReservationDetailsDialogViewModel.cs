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
        public string OnName { get; }
        public string TripType { get; }
        public string NumberOfSeats { get; }
        public string ReservedBy { get; }
        public string TotalPrice { get; }

        private readonly List<FlightModel> _flights;
        public ListCollectionView AddedFlightsCollection { get; }
        [ObservableProperty]
        private int selectedAddedFlightIndex;

        private readonly static string DETAILS_PLACEHOLDER_VALUE = "-";

        public TripReservationDetailsDialogViewModel(TripReservationService tripReservationService)
        {
            var tripReservation = tripReservationService.GetSelectedTripReservation();
            _flights = new List<FlightModel>(tripReservation?.Flights.Select(x => Mapper.Map(x).ToANew<FlightModel>()) ?? Array.Empty<FlightModel>());
            AddedFlightsCollection = new ListCollectionView(_flights);

            OnName = tripReservation?.OnName ?? DETAILS_PLACEHOLDER_VALUE;
            NumberOfSeats = tripReservation?.NumberOfSeats.ToString() ?? DETAILS_PLACEHOLDER_VALUE;
            TripType = tripReservation?.Type.ToString().ToLower().Replace('_', ' ') ?? DETAILS_PLACEHOLDER_VALUE;
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
