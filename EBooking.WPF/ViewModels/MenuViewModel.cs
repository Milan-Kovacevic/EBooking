using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class MenuViewModel : ObservableObject, IViewModelBase
    {
        public record class MenuItem
        {
            public string Name { get; }
            public PackIconKind Kind { get; }
            public IRelayCommand NavigateCommand { get; }

            public MenuItem(string name, PackIconKind kind, Action navigationAction, Func<bool>? canExecute = null)
            {
                Name = name;
                Kind = kind;
                NavigateCommand = new RelayCommand(navigationAction, canExecute ??= () => true);
            }
        }
        public IEnumerable<MenuItem> MenuItems { get; }

        [ObservableProperty]
        private MenuItem selectedItem;

        private readonly NavigationService _navigateToSettingsViewModel;
        private readonly NavigationService _navigateToLoginViewModel;
        private readonly NavigationService _navigateToRegisterViewModel;

        public MenuViewModel(NavigationService navigateToSettingsViewModel, NavigationService navigateToLoginViewModel, NavigationService navigateToRegisterViewModel)
        {
            _navigateToSettingsViewModel = navigateToSettingsViewModel;
            _navigateToLoginViewModel = navigateToLoginViewModel;
            _navigateToRegisterViewModel = navigateToRegisterViewModel;

            MenuItems = new List<MenuItem>()
            {
                new MenuItem("Login", PackIconKind.Login, NavigateToLogin),
                new MenuItem("Register", PackIconKind.Register, NavigateToRegister),
                new MenuItem("Settings", PackIconKind.Settings, NavigateToSettings),
            };
            selectedItem = MenuItems.ElementAt(0);
        }

        public void NavigateToLogin()
        {
            _navigateToLoginViewModel.Navigate();
            SelectedItem = MenuItems.ElementAt(0);
        }

        public void NavigateToRegister()
        {
            _navigateToRegisterViewModel.Navigate();
            SelectedItem = MenuItems.ElementAt(1);
        }

        public void NavigateToSettings()
        {
            _navigateToSettingsViewModel.Navigate();
            SelectedItem = MenuItems.ElementAt(2);
        }
    }
}
