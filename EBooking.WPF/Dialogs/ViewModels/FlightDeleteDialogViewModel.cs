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
    public partial class FlightDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly FlightService _flightService;
        private readonly DialogHostService _dialogHostService;

        public FlightDeleteDialogViewModel(FlightService flightService, DialogHostService dialogHostService)
        {
            _flightService = flightService;
            _dialogHostService = dialogHostService;
            messageText = "Are you sure you want to delete selected flight?";
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentFlight = _flightService.GetSelectedFlight();
            if (currentFlight is not null)
                await _flightService.DeleteFlight(currentFlight.FlightId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
