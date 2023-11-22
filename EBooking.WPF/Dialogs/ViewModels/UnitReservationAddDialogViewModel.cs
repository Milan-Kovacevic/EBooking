using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
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
    public partial class UnitReservationAddDialogViewModel : ObservableValidator, IViewModelBase
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
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationFromDateOnAdd))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? reservationFrom;
        partial void OnReservationFromChanged(DateTime? value)
        {
            if(ReservationTo is not null)
                ValidateProperty(ReservationTo, nameof(ReservationTo));
            if (!HasErrors)
                CalculateTotalPrice();
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationToDateOnAdd))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? reservationTo;
        partial void OnReservationToChanged(DateTime? value)
        {
            if (!HasErrors)
                CalculateTotalPrice();
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string numberOfAdults;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string numberOfChildren;

        [ObservableProperty]
        private string totalPrice;

        public AccommodationUnit? SelectedUnit { get; }
        public string UnitName { get => SelectedUnit?.Name ?? string.Empty; }
        public string UnitAvailability { get => $"{SelectedUnit?.AvailableFrom.ToLongDateString()} - {SelectedUnit?.AvailableTo.ToLongDateString()}"; }
        public decimal PricePerNight { get => SelectedUnit?.PricePerNight ?? 0.0m; }

        public IRelayCommand SubmitCommand { get; }
        private readonly UnitReservationService _unitReservationService;
        private readonly DialogHostService _dialogHostService;
        private readonly UserStore _userStore;
        private readonly AccommodationUnitStore _accommodationUnitStore;

        public UnitReservationAddDialogViewModel(UnitReservationService unitReservationService, UserStore userStore, AccommodationUnitStore accommodationUnitStore, DialogHostService dialogHostService)
        {
            _unitReservationService = unitReservationService;
            _userStore = userStore;
            _accommodationUnitStore = accommodationUnitStore;
            _dialogHostService = dialogHostService;
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_RESERVATION_ADD_DIALOG_TITLE);
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

            onName = string.Empty;
            reservationFrom = null;
            reservationTo = null;
            numberOfAdults = string.Empty;
            numberOfChildren = string.Empty;
            totalPrice = "0.0";
            SelectedUnit = accommodationUnitStore.SelectedAccommodationUnit;
        }

        private bool CanSubmit()
        {
            return OnName != string.Empty
                && ReservationFrom != null
                && ReservationTo != null
                && int.TryParse(NumberOfAdults, out var _)
                && int.TryParse(NumberOfChildren, out var _)
                && !HasErrors;
        }

        private async Task Submit()
        {
            var unitReservation = new UnitReservation()
            {
                OnName = OnName,
                EmployeeId = _userStore.CurrentUser?.UserId ?? 0,
                UnitId = _accommodationUnitStore.SelectedAccommodationUnit?.UnitId ?? 0,
                ReservationFrom = ReservationFrom ?? DateTime.Now,
                ReservationTo = ReservationTo ?? DateTime.Now,
                NumberOfAdults = int.Parse(NumberOfAdults),
                NumberOfChildren = int.Parse(NumberOfChildren),
                TotalPrice = decimal.Parse(TotalPrice)
            };
            await _unitReservationService.AddUnitReservation(unitReservation);
            _dialogHostService.CloseDialogHost();
        }

        private void CalculateTotalPrice()
        {
            if (ReservationFrom == null || ReservationTo == null)
                return;
            int numOfDays = ReservationTo.Value.Subtract(ReservationFrom.Value).Days;
            TotalPrice = $"{numOfDays * PricePerNight}";
        }
    }
}
