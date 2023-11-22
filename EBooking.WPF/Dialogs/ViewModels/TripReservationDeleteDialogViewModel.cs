using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class TripReservationDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly TripReservationService _tripReservationService;
        private readonly DialogHostService _dialogHostService;

        public TripReservationDeleteDialogViewModel(TripReservationService tripReservationService, DialogHostService dialogHostService)
        {
            messageText = LanguageTranslator.Translate(LanguageTranslator.MessageType.TRIP_RESERVATION_DELETE_DIALOG_MESSAGE);
            _tripReservationService = tripReservationService;
            _dialogHostService = dialogHostService;
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentTripReservation = _tripReservationService.GetSelectedTripReservation();
            if (currentTripReservation is not null)
                await _tripReservationService.DeleteTripReservation(currentTripReservation.TripReservationId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
