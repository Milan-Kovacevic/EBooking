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
    public partial class AccommodationUnitDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly AccommodationUnitService _accommodationUnitService;
        private readonly DialogHostService _dialogHostService;

        public AccommodationUnitDeleteDialogViewModel(AccommodationUnitService accommodationUnitService, DialogHostService dialogHostService)
        {
            _accommodationUnitService = accommodationUnitService;
            _dialogHostService = dialogHostService;
            messageText = "Are you sure you want to delete selected accommodation unit?";
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentAccommodationUnit = _accommodationUnitService.GetSelectedAccommodationUnit();
            if (currentAccommodationUnit is not null)
                await _accommodationUnitService.DeleteAccommodationUnit(currentAccommodationUnit.UnitId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
