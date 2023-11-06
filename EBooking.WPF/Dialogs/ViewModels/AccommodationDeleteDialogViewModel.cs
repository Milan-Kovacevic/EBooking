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
    public partial class AccommodationDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly AccommodationService _accommodationService;
        private readonly DialogHostService _dialogHostService;

        public AccommodationDeleteDialogViewModel(AccommodationService accommodationService, DialogHostService dialogHostService)
        {
            _accommodationService = accommodationService;
            _dialogHostService = dialogHostService;
            messageText = "Are you sure you want to delete selected accommodation?";
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentAccommodation = _accommodationService.GetSelectedAccommodation();
            if (currentAccommodation is not null)
                await _accommodationService.DeleteAccommodation(currentAccommodation.AccommodationId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
