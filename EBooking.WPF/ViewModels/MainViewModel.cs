using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace EBooking.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject, IViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly UserStore _userStore;
        private readonly SettingsService _settingsService;
        private readonly UserService _userService;
        private readonly NavigationService _navigateToLandingViewModel;
        private readonly DialogHostService _dialogHostService;

        [ObservableProperty]
        private bool isDarkMode;
        partial void OnIsDarkModeChanged(bool value)
        {
            _settingsService.ChangeBaseTheme(value);
            _settingsService.SaveCurrentSettings();
        }

        [ObservableProperty]
        private IViewModelBase currentViewModel;
        [ObservableProperty]
        private bool isLogoutEnabled;

        public ISnackbarMessageQueue MainMessageQueue { get; }

        public MenuViewModel MenuBinding { get; set; }

        public MainViewModel(MessageQueueStore messageQueueStore, NavigationStore navigationStore, UserStore userStore, UserService userService, SettingsService settingsService, MenuViewModel menuViewModel, NavigationService navigateToLandingViewModel, DialogHostService dialogHostService)
        {
            _navigationStore = navigationStore;
            _userStore = userStore;
            _userService = userService;
            _settingsService = settingsService;
            _dialogHostService = dialogHostService;
            _navigateToLandingViewModel = navigateToLandingViewModel;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChangedAction;
            _userStore.CurrentUserChanged += OnCurrentUserChanged;
            currentViewModel = _navigationStore.CurrentViewModel;
            settingsService.ApplyCurrentSettings();
            MenuBinding = menuViewModel;
            isDarkMode = settingsService.IsDarkThemeSet();
            isLogoutEnabled = false;
            MainMessageQueue = messageQueueStore.SnackbarMessageQueue;
        }

        private void OnCurrentViewModelChangedAction()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
            MenuBinding.CurrentViewModelKey = CurrentViewModel.GetId();
        }

        private void OnCurrentUserChanged()
        {
            if (_userService.IsLoggedIn())
                IsLogoutEnabled = true;
            else
                IsLogoutEnabled = false;
        }

        [RelayCommand]
        public async Task ExitApplication()
        {
            await _dialogHostService.ShowExitApplicationDialog();
        }

        [RelayCommand]
        public void Logout()
        {
            _navigateToLandingViewModel.Navigate();
            if (_navigateToLandingViewModel.CanNavigate())
                _userService.Logout();
        }

        [RelayCommand]
        public void Home()
        {
            _navigateToLandingViewModel.Navigate();
        }

        public void Dispose()
        {
            CurrentViewModel?.Dispose();
            MenuBinding?.Dispose();
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChangedAction;
            _userStore.CurrentUserChanged -= OnCurrentUserChanged;
        }
    }
}
