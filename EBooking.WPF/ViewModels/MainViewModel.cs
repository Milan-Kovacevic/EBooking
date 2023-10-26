using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace EBooking.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject, IViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly SettingsService _settingsService;

        [ObservableProperty]
        private bool isDarkMode;
        partial void OnIsDarkModeChanged(bool value)
        {
            _settingsService.ChangeBaseTheme(value);
            _settingsService.SaveCurrentSettings();
        }

        [ObservableProperty]
        private IViewModelBase currentViewModel;
        public MenuViewModel MenuBinding { get; set; }

        public MainViewModel(NavigationStore navigationStore, SettingsService settingsService, MenuViewModel menuViewModel)
        {
            _navigationStore = navigationStore;
            currentViewModel = _navigationStore.CurrentViewModel;
            _settingsService = settingsService;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChangedAction;
            settingsService.ApplyCurrentSettings();
            MenuBinding = menuViewModel;

            isDarkMode = settingsService.IsDarkThemeSet();
        }

        private void OnCurrentViewModelChangedAction()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
        }

        [RelayCommand]
        public async Task ExitApplication()
        {
            var dialogContent = new ConfirmExitDialog();
            await DialogHost.Show(dialogContent, "RootDialog");
        }

        public void Dispose()
        {
            CurrentViewModel?.Dispose();
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChangedAction;
        }
    }
}
