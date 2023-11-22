using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Dialogs.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class TripReservationAddDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;

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
        private readonly ObservableCollection<FlightModel> _addedFlights;
        public ListCollectionView AddedFlightsCollection { get; set; }

        private readonly FlightStore _flightStore;
        private readonly TripReservationService _tripReservationService;
        private readonly DialogHostService _dialogHostService;
        private readonly UserStore _userStore;

        public TripReservationAddDialogViewModel(FlightStore flightStore, TripReservationService tripReservationService, DialogHostService dialogHostService, UserStore userStore)
        {
            _flightStore = flightStore;
            _tripReservationService = tripReservationService;
            _dialogHostService = dialogHostService;
            _userStore = userStore;

            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.TRIP_RESERVATION_ADD_DIALOG_TITLE);
            TripTypes = new List<TripTypeModel>()
            {
                new TripTypeModel(TripType.ONE_WAY),
                new TripTypeModel(TripType.ROUND_TRIP),
                new TripTypeModel(TripType.MULTI_CITY),
            };
            AvailableFlights = new List<FlightModel>();
            _addedFlights = new ObservableCollection<FlightModel>();
            AddedFlightsCollection = new ListCollectionView(_addedFlights);
            selectedAddedFlightIndex = -1;
            selectedFlightDate = null;
            selectedFlight = null;
            onName = string.Empty;
            numberOfSeats = string.Empty;
            selectedTripType = null;
            totalPrice = "0.0";
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
            await _tripReservationService.AddTripReservation(new TripReservation()
            {
                OnName = OnName,
                EmployeeId = _userStore.CurrentUser?.UserId ?? 0,
                NumberOfSeats = int.Parse(NumberOfSeats),
                TotalPrice = decimal.Parse(TotalPrice),
                Type = SelectedTripType?.Type ?? Domain.Enums.TripType.ONE_WAY,
                Flights = flights
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
