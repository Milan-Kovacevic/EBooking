using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
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
        }

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationToDateOnAdd))]
        [NotifyDataErrorInfo]
        private DateTime? reservationTo;

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

        public IRelayCommand SubmitCommand { get; }
        private readonly DialogHostService _dialogHostService;

        public UnitReservationAddDialogViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = "Add Accommodation Unit Reservation";
            onName = string.Empty;
            reservationFrom = null;
            reservationTo = null;
            numberOfAdults = string.Empty;
            numberOfChildren = string.Empty;
            totalPrice = string.Empty;
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
            _dialogHostService.CloseDialogHost();
        }

    }
}
