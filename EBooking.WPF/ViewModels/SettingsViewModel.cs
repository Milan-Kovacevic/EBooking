using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private List<LanguageItem> availableLanguages;
        [ObservableProperty]
        private List<ColorItem> availablePrimaryColors;

        [ObservableProperty]
        private LanguageItem? selectedLanguageItem;
        [ObservableProperty]
        private int selectedLanguageIndex;
        partial void OnSelectedLanguageIndexChanged(int value)
        {
            if (value < 0)
                return;
            _settingsService.ChangeLanguage(AvailableLanguages.ElementAt(value).Key);
            AvailableLanguages = new List<LanguageItem>(LanguageProvider.Instance.Languages);
            SelectedLanguageItem = AvailableLanguages.ElementAt(value);
        }

        [ObservableProperty]
        private ColorItem? selectedPrimaryColorItem;
        [ObservableProperty]
        private int selectedPrimaryColorIndex;
        partial void OnSelectedPrimaryColorIndexChanged(int value)
        {
            if (value < 0)
                return;
            _settingsService.ChangePrimaryColor(AvailablePrimaryColors.ElementAt(value).Value);
            AvailablePrimaryColors = new List<ColorItem>(ThemeProvider.Instance.PrimaryColors);
            SelectedPrimaryColorItem = AvailablePrimaryColors.ElementAt(value);
        }

        [RelayCommand]
        public void SaveSettings()
        {
            _settingsService.SaveCurrentSettings();
            RebindSettingsProperties();
        }

        [RelayCommand]
        public void RevertSettings()
        {
            _settingsService.RevertSavedSettings();
            RebindSettingsProperties();
        }

        private readonly SettingsStore _settingsStore;
        private readonly SettingsService _settingsService;

        public SettingsViewModel(SettingsStore settingsStore, SettingsService settingsService)
        {
            _settingsService = settingsService;
            _settingsStore = settingsStore;
            availableLanguages = new();
            availablePrimaryColors = new();
            RebindSettingsProperties();
        }

        private void RebindSettingsProperties()
        {
            AvailableLanguages = new List<LanguageItem>(LanguageProvider.Instance.Languages);
            var langaugeCode = _settingsStore.CurrentSettings.LanguageCode;

            var languageItem = AvailableLanguages.First(x => x.Key == langaugeCode);
            SelectedLanguageItem = languageItem;
            SelectedLanguageIndex = AvailableLanguages.FindIndex(l => l.Key == langaugeCode);

            AvailablePrimaryColors = new List<ColorItem>(ThemeProvider.Instance.PrimaryColors);
        }

        public void Dispose()
        {
            RevertSettings();
        }
    }
}
