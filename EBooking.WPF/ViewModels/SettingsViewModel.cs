using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
        private string firstName = string.Empty;
        [ObservableProperty]
        private string lastName = string.Empty;
        [ObservableProperty]
        private string displayName = string.Empty;
        [ObservableProperty]
        private string username = string.Empty;
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

        public bool IsUserLoggedIn { get; }
        public bool IsAdmin { get; }
        public bool IsEmployee { get; }

        [RelayCommand]
        public void SaveSettings()
        {
            if (!SettingsChanged)
                return;
            _settingsService.SaveCurrentSettings();
            RebindSettingsProperties();
            SettingsChanged = false;

            _messageQueueService.Enqueue(LanguageTranslator.MessageType.SUCCESSFUL_SETTINGS_CHANGE);
        }

        [RelayCommand]
        public void RevertSettings()
        {
            _settingsService.RevertSavedSettings();
            RebindSettingsProperties();
            SettingsChanged = false;
        }

        [RelayCommand]
        public void ChangePassword()
        {
            _dialogHostService.OpenChangePasswordDialog();
        }

        private readonly SettingsStore _settingsStore;
        private readonly MessageQueueService _messageQueueService;
        private readonly SettingsService _settingsService;
        private readonly DialogHostService _dialogHostService;
        private readonly UserStore _userStore;

        public SettingsViewModel(MessageQueueService messageQueueService, SettingsStore settingsStore, SettingsService settingsService, DialogHostService dialogHostService, UserStore userStore)
        {
            _settingsService = settingsService;
            _messageQueueService = messageQueueService;
            _settingsStore = settingsStore;
            _dialogHostService = dialogHostService;
            _userStore = userStore;
            _userStore.UserUpdated += OnUserUpdated;
            availableLanguages = new();
            availablePrimaryColors = new();
            availableSecondaryColors = new();
            IsUserLoggedIn = userStore.IsLoggedIn;
            IsAdmin = userStore.IsAdmin;
            IsEmployee = userStore.IsEmployee;
            RebindSettingsProperties();
            if (userStore.IsLoggedIn && userStore.CurrentUser is not null)
                RebindUserInformation(userStore.CurrentUser);
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

        private void RebindUserInformation(User user)
        {
            if (user is Employee employee)
            {
                FirstName = employee?.FirstName ?? string.Empty;
                LastName = employee?.LastName ?? string.Empty;
                Username = employee?.Username ?? string.Empty;
                DisplayName = string.Empty;
            }
            else if (user is Administrator admin)
            {
                FirstName = string.Empty;
                LastName = string.Empty;
                DisplayName = admin?.Name ?? string.Empty;
                Username = admin?.Username ?? string.Empty;
            }
        }

        public void OnUserUpdated(User user)
        {
            RebindUserInformation(user);
            _messageQueueService.Enqueue(LanguageTranslator.MessageType.SUCCESSFUL_PASSWORD_CHANGE);
        }

        public void Dispose()
        {
            _userStore.UserUpdated -= OnUserUpdated;
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.SETTINGS);

        public bool CanNavigateFrom()
        {
            if (SettingsChanged)
            {
                _dialogHostService.OpenSaveSettingsDialog();
                return false;
            }
            return true;
        }
    }
}
