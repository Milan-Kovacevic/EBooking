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
        private readonly DialogNavigationStore _dialogNavigationStore;
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
        private IViewModelBase? currentDialogViewModel;
        [ObservableProperty]
        private bool isDialogOpen;
        [ObservableProperty]
        private bool isLogoutEnabled;

        public ISnackbarMessageQueue MainMessageQueue { get; }

        public MenuViewModel MenuBinding { get; set; }

        public MainViewModel(MessageQueueStore messageQueueStore, NavigationStore navigationStore, DialogNavigationStore dialogNavigationStore, UserStore userStore, UserService userService, SettingsService settingsService, MenuViewModel menuViewModel, NavigationService navigateToLandingViewModel, DialogHostService dialogHostService)
        {
            _navigationStore = navigationStore;
            _dialogNavigationStore = dialogNavigationStore;
            _userStore = userStore;
            _userService = userService;
            _settingsService = settingsService;
            _dialogHostService = dialogHostService;
            _navigateToLandingViewModel = navigateToLandingViewModel;

            _userStore.CurrentUserChanged += OnCurrentUserChanged;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChangedAction;
            _dialogNavigationStore.CurrentDialogViewModelChanged += OnCurrentDialogViewModelChangedAction;
            currentViewModel = _navigationStore.CurrentViewModel;
            currentDialogViewModel = _dialogNavigationStore.CurrentDialogViewModel;
            settingsService.ApplyCurrentSettings();
            MenuBinding = menuViewModel;
            isDarkMode = settingsService.IsDarkThemeSet();
            isLogoutEnabled = false;
            isDialogOpen = false;
            MainMessageQueue = messageQueueStore.SnackbarMessageQueue;
        }

        private void OnCurrentViewModelChangedAction()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
            MenuBinding.CurrentViewModelKey = CurrentViewModel.GetId();
        }

        private void OnCurrentDialogViewModelChangedAction()
        {
            CurrentDialogViewModel = _dialogNavigationStore.CurrentDialogViewModel;
            if (CurrentDialogViewModel is not null)
                IsDialogOpen = true;
        }

        private void OnCurrentUserChanged()
        {
            if (_userService.IsLoggedIn())
                IsLogoutEnabled = true;
            else
                IsLogoutEnabled = false;
        }

        [RelayCommand]
        public void ExitApplication()
        {
            _dialogHostService.OpenExitApplicationDialog();
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
            _dialogNavigationStore.CurrentDialogViewModelChanged -= OnCurrentDialogViewModelChangedAction;
            _userStore.CurrentUserChanged -= OnCurrentUserChanged;
        }
    }
}
