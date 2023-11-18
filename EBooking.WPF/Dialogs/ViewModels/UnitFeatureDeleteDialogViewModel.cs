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
    public partial class UnitFeatureDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string messageText;

        private readonly UnitFeaturesService _unitFeaturesService;
        private readonly DialogHostService _dialogHostService;

        public UnitFeatureDeleteDialogViewModel(UnitFeaturesService unitFeaturesService, DialogHostService dialogHostService)
        {
            _unitFeaturesService = unitFeaturesService;
            _dialogHostService = dialogHostService;
            messageText = LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_FEATURE_DELETE_DIALOG_MESSAGE);
        }

        [RelayCommand]
        public async Task Delete()
        {
            var currentFeature = _unitFeaturesService.GetSelectedUnitFeature();
            if (currentFeature is not null)
                await _unitFeaturesService.DeleteUnitFeature(currentFeature.FeatureId);

            _dialogHostService.CloseDialogHost();
        }
    }
}
