using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class LocationDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly LocationsService _locationsService;
        private readonly DialogHostService _dialogHostService;

        public LocationDeleteDialogViewModel(LocationsService locationsService, DialogHostService dialogHostService)
        {
            _locationsService = locationsService;
            _dialogHostService = dialogHostService;
            messageText = LanguageTranslator.Translate(LanguageTranslator.MessageType.LOCATION_DELETE_DIALOG_MESSAGE);
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentLocation = _locationsService.GetSelectedLocation();
            if (currentLocation is not null)
                await _locationsService.DeleteLocation(currentLocation.LocationId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
