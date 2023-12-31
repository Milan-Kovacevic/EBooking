﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class UnitFeatureAddDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string featureName;

        private readonly UnitFeaturesService _unitFeatureService;
        private readonly DialogHostService _dialogHostService;
        public IRelayCommand SubmitCommand { get; }

        public UnitFeatureAddDialogViewModel(UnitFeaturesService unitFeatureService, DialogHostService dialogHostService)
        {
            _unitFeatureService = unitFeatureService;
            _dialogHostService = dialogHostService;
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_FEATURE_ADD_DIALOG_TITLE);
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

            featureName = string.Empty;
        }

        private bool CanSubmit()
        {
            return FeatureName != string.Empty && !HasErrors;
        }

        private async Task Submit()
        {
            await _unitFeatureService.AddUnitFeature(FeatureName);
            _dialogHostService.CloseDialogHost();
        }
    }
}
