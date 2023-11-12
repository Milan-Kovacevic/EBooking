using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class TripReservationAddDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string onName;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        private string numberOfSeats;

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
        public ObservableCollection<FlightModel> AddedFlights { get; }

        private readonly FlightStore _flightStore;
        private readonly DialogHostService _dialogHostService;

        public TripReservationAddDialogViewModel(FlightStore flightStore, DialogHostService dialogHostService)
        {
            _flightStore = flightStore;
            _dialogHostService = dialogHostService;
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = "Create Trip Reservation";
            TripTypes = new List<TripTypeModel>()
            {
                new TripTypeModel(Domain.Enums.TripType.ONE_WAY),
                new TripTypeModel(Domain.Enums.TripType.ROUND_TRIP),
                new TripTypeModel(Domain.Enums.TripType.MULTI_CITY),
            };
            AvailableFlights = new List<FlightModel>();
            AddedFlights = new ObservableCollection<FlightModel>();

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
                && !HasErrors;
        }

        private async Task Submit()
        {
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
            if (SelectedFlight != null && !AddedFlights.Contains(SelectedFlight))
                AddedFlights.Add(SelectedFlight);
        }

        [RelayCommand]
        public void RemoveSelectedFlight()
        {
            if (SelectedAddedFlightIndex < 0 || SelectedAddedFlightIndex >= AddedFlights.Count)
                return;
            AddedFlights.RemoveAt(SelectedAddedFlightIndex);
        }
    }
}
