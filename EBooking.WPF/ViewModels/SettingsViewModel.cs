using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, IViewModelBase
    {
        public string Key => nameof(SettingsViewModel);

        [ObservableProperty]
        private bool isDarkMode;
        partial void OnIsDarkModeChanged(bool value)
        {
            _settingsService.ChangeBaseTheme(value);
        }

        [RelayCommand]
        private void SaveSettings()
        {
            _settingsService.SaveCurrentSettings();
        }

        private readonly SettingsService _settingsService;

        public SettingsViewModel(SettingsStore settingsStore, SettingsService settingsService)
        {
            _settingsService = settingsService;
            var settings = settingsStore.CurrentSettings;
            isDarkMode = settings.BaseTheme == Theme.Dark;
        }
    }
}
