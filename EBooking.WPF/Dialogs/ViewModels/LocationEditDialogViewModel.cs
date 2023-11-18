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
    public partial class LocationEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;
        public int LocationId { get; set; }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string countryName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string cityName;

        private readonly LocationsService _locationsService;
        private readonly DialogHostService _dialogHostService;
        public IRelayCommand SubmitCommand { get; }

        public LocationEditDialogViewModel(LocationsService locationsService, DialogHostService dialogHostService)
        {
            _locationsService = locationsService;
            _dialogHostService = dialogHostService;
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.LOCATION_EDIT_DIALOG_TITLE);
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

            var location = locationsService.GetSelectedLocation();
            countryName = location?.Country ?? string.Empty;
            cityName = location?.City ?? string.Empty;
            LocationId = location?.LocationId ?? 0;
        }

        private bool CanSubmit()
        {
            return CountryName != string.Empty
                && CityName != string.Empty
                && !HasErrors;
        }

        private async Task Submit()
        {
            await _locationsService.UpdateLocation(LocationId, CountryName, CityName);
            _dialogHostService.CloseDialogHost();
        }
    }
}
