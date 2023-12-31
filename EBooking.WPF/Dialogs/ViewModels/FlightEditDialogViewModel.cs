﻿using AgileObjects.AgileMapper;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class FlightEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;
        public int FlightId { get; set; }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string avioCompanyName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private FlightClassModel? flightClassModel;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private LocationModel? fromLocation;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private LocationModel? toLocation;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveDecimalNumber))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string ticketPrice;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateDepartureDate))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? departureDate;
        partial void OnDepartureDateChanged(DateTime? value)
        {
            if (ArrivalDate != null)
                ValidateProperty(ArrivalDate, nameof(ArrivalDate));
            if (ArrivalTime != null)
                ValidateProperty(ArrivalTime, nameof(ArrivalTime));
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateArrivalDateOnEdit))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? arrivalDate;
        partial void OnArrivalDateChanged(DateTime? value)
        {
            if (ArrivalTime != null)
                ValidateProperty(ArrivalTime, nameof(ArrivalTime));
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? departureTime;
        partial void OnDepartureTimeChanged(DateTime? value)
        {
            if (ArrivalTime != null)
                ValidateProperty(ArrivalTime, nameof(ArrivalTime));
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateArrivalTimeOnEdit))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? arrivalTime;

        public IRelayCommand SubmitCommand { get; }
        public IEnumerable<FlightClassModel> FlightClasses { get; }
        public IEnumerable<LocationModel> Locations { get; }

        private readonly LocationsStore _locationsStore;
        private readonly DialogHostService _dialogHostService;
        private readonly FlightService _flightService;
        private readonly UserStore _userStore;

        public FlightEditDialogViewModel(LocationsStore locationsStore, FlightService flightService, UserStore userStore, DialogHostService dialogHostService)
        {
            _locationsStore = locationsStore;
            _flightService = flightService;
            _userStore = userStore;
            _dialogHostService = dialogHostService;

            Locations = _locationsStore.Locations.Select(x => Mapper.Map(x).ToANew<LocationModel>());
            FlightClasses = new List<FlightClassModel>()
            {
                new FlightClassModel(FlightClass.STANDARD),
                new FlightClassModel(FlightClass.BUSINESS),
                new FlightClassModel(FlightClass.ECONOMY),
                new FlightClassModel(FlightClass.PERMIUM_ECONOMY),
                new FlightClassModel(FlightClass.ECO_FLY),
            };
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.FLIGHT_EDIT_DIALOG_TITLE);

            var flight = flightService.GetSelectedFlight();
            FlightId = flight?.FlightId ?? 0;
            avioCompanyName = flight?.AvioCompanyName ?? string.Empty;
            flightClassModel = new FlightClassModel(flight?.FlightClass ?? FlightClass.STANDARD);
            fromLocation = Mapper.Map(flight?.FromLocation).ToANew<LocationModel>();
            toLocation = Mapper.Map(flight?.ToLocation).ToANew<LocationModel>();
            ticketPrice = flight?.TicketPrice.ToString() ?? "0.0";
            departureDate = flight?.DepartureTime;
            departureTime = flight?.DepartureTime;
            arrivalDate = flight?.ArrivalTime;
            arrivalTime = flight?.ArrivalTime;
        }

        private bool CanSubmit()
        {
            return AvioCompanyName != string.Empty
                && FlightClassModel != null
                && FromLocation != null
                && ToLocation != null
                && DepartureDate != null
                && DepartureTime != null
                && ArrivalDate != null
                && ArrivalTime != null
                && decimal.TryParse(TicketPrice, out _)
                && !HasErrors;
        }

        private async Task Submit()
        {
            var times = GetDepartureAndArrivalTimes();
            await _flightService.UpdateFlight(new Flight()
            {
                FlightId = FlightId,
                AvioCompanyName = AvioCompanyName,
                UserId = _userStore.CurrentUser?.UserId ?? 0,
                FlightClass = FlightClassModel?.FlightClass ?? FlightClass.STANDARD,
                FromLocationId = FromLocation?.LocationId ?? 0,
                ToLocationId = ToLocation?.LocationId ?? 0,
                TicketPrice = decimal.Parse(TicketPrice),
                DepartureTime = times.Item1,
                ArrivalTime = times.Item2,
            });
            _dialogHostService.CloseDialogHost();
        }

        private (DateTime, DateTime) GetDepartureAndArrivalTimes()
        {
            if (DepartureDate == null
                || DepartureTime == null
                || ArrivalDate == null
                || ArrivalTime == null)
                return (DateTime.Now, DateTime.Now);

            var departDate = (DateTime)DepartureDate;
            var departTime = (DateTime)DepartureTime;
            var departureDate = new DateTime(departDate.Year, departDate.Month, departDate.Day, departTime.Hour, departTime.Minute, departTime.Second);
            var arriveDate = (DateTime)ArrivalDate;
            var arriveTime = (DateTime)ArrivalTime;
            var arrivalDate = new DateTime(arriveDate.Year, arriveDate.Month, arriveDate.Day, arriveTime.Hour, arriveTime.Minute, arriveTime.Second);

            return (departureDate, arrivalDate);
        }
    }
}
