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
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string onName;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationFromDate))]
        [NotifyDataErrorInfo]
        private DateTime? reservationFrom;
        partial void OnReservationFromChanged(DateTime? value)
        {
            ValidateProperty(ReservationTo, nameof(ReservationTo));
            if (!HasErrors)
                CalculateTotalPrice();
        }

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationToDateOnAdd))]
        [NotifyDataErrorInfo]
        private DateTime? reservationTo;
        partial void OnReservationToChanged(DateTime? value)
        {
            if (!HasErrors)
                CalculateTotalPrice();
        }

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        private string numberOfAdults;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        private string numberOfChildren;

        [ObservableProperty]
        private string totalPrice;
        public string UnitName { get; }
        public decimal PricePerNight { get; }

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

            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = "Add Accommodation Unit Reservation";
            onName = string.Empty;
            reservationFrom = null;
            reservationTo = null;
            numberOfAdults = string.Empty;
            numberOfChildren = string.Empty;
            totalPrice = "0.0";
            PricePerNight = accommodationUnitStore.SelectedAccommodationUnit?.PricePerNight ?? 0.0m;
            UnitName = accommodationUnitStore.SelectedAccommodationUnit?.Name ?? string.Empty;
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
