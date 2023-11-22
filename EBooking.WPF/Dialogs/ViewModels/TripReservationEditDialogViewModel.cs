using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using EBooking.WPF.Dialogs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;
using EBooking.WPF.Stores;
using EBooking.WPF.Services;
using AgileObjects.AgileMapper;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using AgileObjects.AgileMapper.Extensions;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class TripReservationEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;
        public int TripReservationId { get; set; }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string onName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        private string numberOfSeats;
        partial void OnNumberOfSeatsChanged(string value)
        {
            CalculateTotalPrice();
        }

        [ObservableProperty]
        private string totalPrice;
        [ObservableProperty]
        private TripTypeModel? selectedTripType;

        [ObservableProperty]
        private DateTime? selectedFlightDate;
        partial void OnSelectedFlightDateChanged(DateTime? value)
        {
            FilterAvailableFlights();
        }
        [ObservableProperty]
        private FlightModel? selectedFlight;
        [ObservableProperty]
        private int selectedAddedFlightIndex;

        public IRelayCommand SubmitCommand { get; }
        public IEnumerable<TripTypeModel> TripTypes { get; }
        public List<FlightModel> AvailableFlights { get; }
        private ObservableCollection<FlightModel> _addedFlights;
        public ListCollectionView AddedFlightsCollection { get; set; }

        private readonly FlightStore _flightStore;
        private readonly TripReservationService _tripReservationService;
        private readonly DialogHostService _dialogHostService;
        private readonly UserStore _userStore;

        public TripReservationEditDialogViewModel(FlightStore flightStore, TripReservationService tripReservationService, DialogHostService dialogHostService, UserStore userStore)
        {
            _flightStore = flightStore;
            _tripReservationService = tripReservationService;
            _dialogHostService = dialogHostService;
            _userStore = userStore;

            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.TRIP_RESERVATION_EDIT_DIALOG_TITLE);
            TripTypes = new List<TripTypeModel>()
            {
                new TripTypeModel(TripType.ONE_WAY),
                new TripTypeModel(TripType.ROUND_TRIP),
                new TripTypeModel(TripType.MULTI_CITY),
            };
            AvailableFlights = new List<FlightModel>();
            var tripReservation = tripReservationService.GetSelectedTripReservation();
            _addedFlights = new ObservableCollection<FlightModel>(tripReservation?.Flights.Select(x => Mapper.Map(x).ToANew<FlightModel>()) ?? Array.Empty<FlightModel>());
            AddedFlightsCollection = new ListCollectionView(_addedFlights);

            onName = tripReservation?.OnName ?? string.Empty;
            numberOfSeats = tripReservation?.NumberOfSeats.ToString() ?? string.Empty;
            selectedTripType = new TripTypeModel(tripReservation?.Type ?? TripType.ONE_WAY);
            totalPrice = tripReservation?.TotalPrice.ToString() ?? "0.0";
            selectedAddedFlightIndex = -1;
            TripReservationId = tripReservation?.TripReservationId ?? 0;
            selectedFlightDate = null;
            selectedFlight = null;
            FilterAvailableFlights();
        }

        private bool CanSubmit()
        {
            return OnName != string.Empty
                && int.TryParse(NumberOfSeats, out _)
                && SelectedTripType != null
                && !AddedFlightsCollection.IsEmpty
                && !HasErrors;
        }

        private async Task Submit()
        {
            var flights = new List<Flight>(_addedFlights.Select(x => Mapper.Map(x).ToANew<Flight>()));
            await _tripReservationService.UpdateTripReservation(new TripReservation()
            {
                TripReservationId = TripReservationId,
                OnName = OnName,
                EmployeeId = _userStore.CurrentUser?.UserId ?? 0,
                NumberOfSeats = int.Parse(NumberOfSeats),
                TotalPrice = decimal.Parse(TotalPrice),
                Type = SelectedTripType?.Type ?? TripType.ONE_WAY,
                Flights = flights,
            });
            _dialogHostService.CloseDialogHost();
        }

        private void FilterAvailableFlights()
        {
            AvailableFlights.Clear();
            SelectedFlight = null;
            AvailableFlights.AddRange(_flightStore
                .Flights
                .Where(x => SelectedFlightDate == null || x.DepartureTime.Date == SelectedFlightDate)
                .Select(x => Mapper.Map(x).ToANew<FlightModel>()));
        }

        [RelayCommand]
        public void AddSelectedFlight()
        {
            if (SelectedFlight != null && !_addedFlights.Contains(SelectedFlight))
            {
                _addedFlights.Add(SelectedFlight);
                CalculateTotalPrice();
                SubmitCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        public void RemoveSelectedFlight()
        {
            if (SelectedAddedFlightIndex < 0 || SelectedAddedFlightIndex >= _addedFlights.Count)
                return;
            _addedFlights.RemoveAt(SelectedAddedFlightIndex);
            CalculateTotalPrice();
            SubmitCommand.NotifyCanExecuteChanged();
        }

        private void CalculateTotalPrice()
        {
            if (!int.TryParse(NumberOfSeats, out var num))
                return;
            var totalFlightCost = 0.0m;
            _addedFlights.ForEach(f => totalFlightCost += f.TicketPrice);
            TotalPrice = $"{num * totalFlightCost}";
        }
    }
}
