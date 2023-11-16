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
    public partial class UnitReservationEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        public int UnitReservationId { get; set; }

        [ObservableProperty]
        private string dialogTitle;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string onName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationFromDate))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? reservationFrom;
        partial void OnReservationFromChanged(DateTime? value)
        {
            if (ReservationTo is not null)
                ValidateProperty(ReservationTo, nameof(ReservationTo));
            if (!HasErrors)
                CalculateTotalPrice();
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationToDateOnEdit))]
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

        public string UnitName { get; }
        public string UnitAvailability { get; }
        public decimal PricePerNight { get; }

        public IRelayCommand SubmitCommand { get; }
        private readonly UnitReservationStore _unitReservationStore;
        private readonly UnitReservationService _unitReservationService;
        private readonly DialogHostService _dialogHostService;
        private readonly UserStore _userStore;

        public UnitReservationEditDialogViewModel(UnitReservationStore unitReservationStore, UnitReservationService unitReservationService, UserStore userStore, DialogHostService dialogHostService)
        {
            _unitReservationStore = unitReservationStore;
            _unitReservationService = unitReservationService;
            _userStore = userStore;
            _dialogHostService = dialogHostService;
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_RESERVATION_EDIT_DIALOG_TITLE);
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

            var unitReservation = unitReservationStore.SelectedUnitReservation;
            UnitReservationId = unitReservation?.UnitReservationId ?? 0;
            onName = unitReservation?.OnName ?? string.Empty;
            reservationFrom = unitReservation?.ReservationFrom ?? null;
            reservationTo = unitReservation?.ReservationTo ?? null;
            numberOfAdults = unitReservation?.NumberOfAdults.ToString() ?? string.Empty;
            numberOfChildren = unitReservation?.NumberOfChildren.ToString() ?? string.Empty;
            totalPrice = unitReservation?.TotalPrice.ToString() ?? "0.0";
            var accommodationUnit = unitReservation?.Unit;
            PricePerNight = accommodationUnit?.PricePerNight ?? 0.0m;
            UnitName = accommodationUnit?.Name ?? string.Empty;
            UnitAvailability = $"{accommodationUnit?.AvailableFrom.ToLongDateString()} - {accommodationUnit?.AvailableTo.ToLongDateString()}";
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
                UnitReservationId = UnitReservationId,
                OnName = OnName,
                EmployeeId = _userStore.CurrentUser?.UserId ?? 0,
                UnitId = _unitReservationStore?.SelectedUnitReservation?.UnitId ?? 0,
                ReservationFrom = ReservationFrom ?? DateTime.Now,
                ReservationTo = ReservationTo ?? DateTime.Now,
                NumberOfAdults = int.Parse(NumberOfAdults),
                NumberOfChildren = int.Parse(NumberOfChildren),
                TotalPrice = decimal.Parse(TotalPrice)
            };
            await _unitReservationService.UpdateUnitReservation(unitReservation);
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
