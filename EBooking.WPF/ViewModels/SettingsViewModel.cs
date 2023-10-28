using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs;
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
        private bool settingsChanged;
        [ObservableProperty]
        private List<LanguageItem> availableLanguages;
        [ObservableProperty]
        private List<ColorItem> availablePrimaryColors;
        [ObservableProperty]
        private List<ColorItem> availableSecondaryColors;

        [ObservableProperty]
        private LanguageItem? selectedLanguageItem;
        [ObservableProperty]
        private int selectedLanguageIndex;
        partial void OnSelectedLanguageIndexChanged(int value)
        {
            if (value < 0)
                return;
            if (!SettingsChanged)
                SettingsChanged = true;
            _settingsService.ChangeLanguage(AvailableLanguages.ElementAt(value).Key);
            RebindSettingsProperties();
        }

        [ObservableProperty]
        private ColorItem? selectedPrimaryColorItem;
        [ObservableProperty]
        private int selectedPrimaryColorIndex;
        partial void OnSelectedPrimaryColorIndexChanged(int value)
        {
            if (value < 0)
                return;
            if (!SettingsChanged)
                SettingsChanged = true;
            _settingsService.ChangePrimaryColor(AvailablePrimaryColors.ElementAt(value).Key);
        }

        [ObservableProperty]
        private ColorItem? selectedSecondaryColorItem;
        [ObservableProperty]
        private int selectedSecondaryColorIndex;
        partial void OnSelectedSecondaryColorIndexChanged(int value)
        {
            if (value < 0)
                return;
            if (!SettingsChanged)
                SettingsChanged = true;
            _settingsService.ChangeSecondaryColor(AvailableSecondaryColors.ElementAt(value).Key);
        }

        [RelayCommand]
        public void SaveSettings()
        {
            if (!SettingsChanged)
                return;
            _settingsService.SaveCurrentSettings();
            RebindSettingsProperties();
            SettingsChanged = false;

            _messageQueueService.Enqueue("Settings saved successfully!");
        }

        [RelayCommand]
        public void RevertSettings()
        {
            _settingsService.RevertSavedSettings();
            RebindSettingsProperties();
            SettingsChanged = false;
        }

        private readonly SettingsStore _settingsStore;
        private readonly MessageQueueService _messageQueueService;
        private readonly SettingsService _settingsService;

        public SettingsViewModel(MessageQueueService messageQueueService, SettingsStore settingsStore, SettingsService settingsService)
        {
            _settingsService = settingsService;
            _messageQueueService = messageQueueService;
            _settingsStore = settingsStore;
            availableLanguages = new();
            availablePrimaryColors = new();
            availableSecondaryColors = new();
            RebindSettingsProperties();
            settingsChanged = false;
        }

        private void RebindSettingsProperties()
        {
            AvailableLanguages = new List<LanguageItem>(LanguageProvider.Instance.Languages);
            var langaugeCode = _settingsStore.CurrentSettings.LanguageCode;
            var languageItem = AvailableLanguages.First(x => x.Key == langaugeCode);
            SelectedLanguageItem = languageItem;
            SelectedLanguageIndex = AvailableLanguages.FindIndex(l => l.Key == langaugeCode);

            var themeProvider = ThemeProvider.Instance;

            AvailablePrimaryColors = new List<ColorItem>(themeProvider.PrimaryColors);
            var primaryColorCode = themeProvider.ResolvePrimaryColorCode(_settingsStore.CurrentSettings.PrimaryColorCode);
            var primaryColorItem = AvailablePrimaryColors.First(x => x.Key == primaryColorCode);
            SelectedPrimaryColorItem = primaryColorItem;
            SelectedPrimaryColorIndex = AvailablePrimaryColors.FindIndex(l => l.Key == primaryColorCode);

            AvailableSecondaryColors = new List<ColorItem>(ThemeProvider.Instance.SecondaryColors);
            var secondaryColorCode = themeProvider.ResolveSecondaryColorCode(_settingsStore.CurrentSettings.SecondaryColorCode);
            var secondaryColorItem = AvailableSecondaryColors.First(x => x.Key == secondaryColorCode);
            SelectedSecondaryColorItem = secondaryColorItem;
            SelectedSecondaryColorIndex = AvailableSecondaryColors.FindIndex(l => l.Key == secondaryColorCode);
        }

        public void Dispose()
        {
            if (SettingsChanged)
                RevertSettings();
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.SETTINGS);

        public bool CanNavigateFrom()
        {
            if (SettingsChanged)
            {
                var dialogContent = new SaveSettingsAlertDialog();
                DialogHost.Show(dialogContent, "RootDialog");
                return false;
            }
            return true;
        }
    }
}
