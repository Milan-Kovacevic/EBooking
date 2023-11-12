using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EBooking.WPF.Messages;
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
    public partial class MenuViewModel : ObservableObject, IViewModelBase, IRecipient<LanguageChangeMessage>
    {
        public partial class MenuViewItem : ObservableObject
        {
            [ObservableProperty]
            private string name;
            [ObservableProperty]
            private bool isEnabled;
            partial void OnIsEnabledChanged(bool value)
            {

            }
            public PackIconKind Kind { get; }
            public IRelayCommand NavigateCommand { get; }

            public MenuViewItem(string name, PackIconKind kind, Action navigationAction, Func<bool>? canExecute = null)
            {
                this.name = name;
                isEnabled = false;
                Kind = kind;
                NavigateCommand = new RelayCommand(navigationAction, canExecute ??= () => true);
            }
        }

        private string currentViewModelKey;
        public string CurrentViewModelKey
        {
            get => currentViewModelKey;
            set
            {
                currentViewModelKey = value;
                int index = MenuProvider.GetIndexOfCode(currentViewModelKey);
                if (index != -1)
                    SelectedItem = MenuItems[index];
                else
                    SelectedItem = null;
            }
        }

        [ObservableProperty]
        private ObservableCollection<MenuViewItem> menuItems;

        [ObservableProperty]
        private MenuViewItem? selectedItem;

        private readonly NavigationService _navigateToSettingsViewModel;
        private readonly NavigationService _navigateToLoginViewModel;
        private readonly NavigationService _navigateToRegisterViewModel;
        private readonly NavigationService _navigateToCodebookViewModel;
        private readonly NavigationService _navigateToAccommodationsViewModel;
        private readonly NavigationService _navigateToFlightsViewModel;
        private readonly NavigationService _navigateToReservationsViewModel;
        private readonly UserStore _userStore;

        public MenuViewModel(UserStore userStore, NavigationService navigateToSettingsViewModel, NavigationService navigateToLoginViewModel, NavigationService navigateToRegisterViewModel, NavigationService navigateToCodebookViewModel, NavigationService navigateToAccommodationsViewModel, NavigationService navigateToFlightsViewModel, NavigationService navigateToReservationsViewModel)
        {
            _navigateToSettingsViewModel = navigateToSettingsViewModel;
            _navigateToLoginViewModel = navigateToLoginViewModel;
            _navigateToRegisterViewModel = navigateToRegisterViewModel;
            _navigateToCodebookViewModel = navigateToCodebookViewModel;
            _navigateToAccommodationsViewModel = navigateToAccommodationsViewModel;
            _navigateToFlightsViewModel = navigateToFlightsViewModel;
            _navigateToReservationsViewModel = navigateToReservationsViewModel;
            _userStore = userStore;
            _userStore.CurrentUserChanged += OnCurrentUserChanged;
            menuItems = new ObservableCollection<MenuViewItem>()
            {
                new MenuViewItem(string.Empty, PackIconKind.Login, NavigateToLogin) { IsEnabled = true},
                new MenuViewItem(string.Empty, PackIconKind.Register, NavigateToRegister) { IsEnabled = true},
                new MenuViewItem(string.Empty, PackIconKind.Hotel, NavigateToAccommodations),
                new MenuViewItem(string.Empty, PackIconKind.Flight, NavigateToFlights),
                new MenuViewItem(string.Empty, PackIconKind.BookOpenPageVariant, NavigateToReservations),
                new MenuViewItem(string.Empty, PackIconKind.BookSearch, NavigateToCodebook),
                new MenuViewItem(string.Empty, PackIconKind.Settings, NavigateToSettings) { IsEnabled = true},
            };
            selectedItem = null;
            currentViewModelKey = string.Empty;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        private void OnCurrentUserChanged()
        {
            if (_userStore.IsAdmin)
                EnableAdminMenuItems();
            else if (_userStore.IsEmployee)
                EnableEmployeeMenuItems();
            else
                EnableLoggedOfUserMenuItems();
        }

        public void NavigateToLogin()
        {
            _navigateToLoginViewModel.Navigate();
        }

        public void NavigateToRegister()
        {
            _navigateToRegisterViewModel.Navigate();
        }

        public void NavigateToSettings()
        {
            _navigateToSettingsViewModel.Navigate();
        }

        public void NavigateToAccommodations()
        {
            _navigateToAccommodationsViewModel.Navigate();
        }

        public void NavigateToFlights()
        {
            _navigateToFlightsViewModel.Navigate();
        }

        public void NavigateToReservations()
        {
            _navigateToReservationsViewModel.Navigate();
        }

        public void NavigateToCodebook()
        {
            _navigateToCodebookViewModel.Navigate();
        }

        #region Helper Methods
        private void EnableAdminMenuItems()
        {
            MenuItems.ElementAt(0).IsEnabled = false;
            MenuItems.ElementAt(1).IsEnabled = true;
            MenuItems.ElementAt(2).IsEnabled = true;
            MenuItems.ElementAt(3).IsEnabled = true;
            MenuItems.ElementAt(4).IsEnabled = true;
            MenuItems.ElementAt(5).IsEnabled = true;
            MenuItems.ElementAt(6).IsEnabled = true;
        }
        private void EnableEmployeeMenuItems()
        {
            MenuItems.ElementAt(0).IsEnabled = false;
            MenuItems.ElementAt(1).IsEnabled = true;
            MenuItems.ElementAt(2).IsEnabled = true;
            MenuItems.ElementAt(3).IsEnabled = true;
            MenuItems.ElementAt(4).IsEnabled = true;
            MenuItems.ElementAt(5).IsEnabled = false;
            MenuItems.ElementAt(6).IsEnabled = true;
        }
        private void EnableLoggedOfUserMenuItems()
        {
            MenuItems.ElementAt(0).IsEnabled = true;
            MenuItems.ElementAt(1).IsEnabled = true;
            MenuItems.ElementAt(2).IsEnabled = false;
            MenuItems.ElementAt(3).IsEnabled = false;
            MenuItems.ElementAt(4).IsEnabled = false;
            MenuItems.ElementAt(5).IsEnabled = false;
            MenuItems.ElementAt(6).IsEnabled = true;
        }
        #endregion

        public void Receive(LanguageChangeMessage message)
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Name = Util.GetLocalizedValue(MenuProvider.GetCodeByIndex(i));
            }
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            _userStore.CurrentUserChanged -= OnCurrentUserChanged;
        }
    }
}
