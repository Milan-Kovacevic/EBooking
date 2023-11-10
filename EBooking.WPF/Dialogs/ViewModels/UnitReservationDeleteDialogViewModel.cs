using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class UnitReservationDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly UnitReservationService _unitReservationService;
        private readonly DialogHostService _dialogHostService;

        public UnitReservationDeleteDialogViewModel(UnitReservationService unitReservationService, DialogHostService dialogHostService)
        {
            _unitReservationService = unitReservationService;
            _dialogHostService = dialogHostService;
            messageText = "Are you sure you want to delete selected unit reservation/s?";
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentUnitReservation = _unitReservationService.GetSelectedUnitReservation();
            if (currentUnitReservation is not null)
                await _unitReservationService.DeleteUnitReservation(currentUnitReservation.UnitReservationId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
