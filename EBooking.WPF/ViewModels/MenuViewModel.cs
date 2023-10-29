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
            public PackIconKind Kind { get; }
            public IRelayCommand NavigateCommand { get; }

            public MenuViewItem(string name, PackIconKind kind, Action navigationAction, Func<bool>? canExecute = null)
            {
                this.name = name;
                isEnabled = true;
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
        private readonly UserStore _userStore;

        public MenuViewModel(UserStore userStore, NavigationService navigateToSettingsViewModel, NavigationService navigateToLoginViewModel, NavigationService navigateToRegisterViewModel, NavigationService navigateToCodebookViewModel)
        {
            _navigateToSettingsViewModel = navigateToSettingsViewModel;
            _navigateToLoginViewModel = navigateToLoginViewModel;
            _navigateToRegisterViewModel = navigateToRegisterViewModel;
            _navigateToCodebookViewModel = navigateToCodebookViewModel;
            _userStore = userStore;
            _userStore.CurrentUserChanged += OnCurrentUserChanged;
            menuItems = new ObservableCollection<MenuViewItem>()
            {
                new MenuViewItem(string.Empty, PackIconKind.Login, NavigateToLogin),
                new MenuViewItem(string.Empty, PackIconKind.Register, NavigateToRegister),
                new MenuViewItem(string.Empty, PackIconKind.Settings, NavigateToSettings),
                new MenuViewItem(string.Empty, PackIconKind.BookAdd, NavigateToCodebook),
            };
            selectedItem = null;
            currentViewModelKey = string.Empty;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        private void OnCurrentUserChanged()
        {
            if(_userStore.IsLoggedIn)
                MenuItems.ElementAt(0).IsEnabled = false;
            else
                MenuItems.ElementAt(0).IsEnabled = true;
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

        public void NavigateToCodebook()
        {
            _navigateToCodebookViewModel.Navigate();
        }

        public void Receive(LanguageChangeMessage message)
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Name = Util.GetLocalizedValue(MenuProvider.GetCodeByIndex(i));
            }
        }

        public string Key => nameof(MenuViewModel);

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            _userStore.CurrentUserChanged -= OnCurrentUserChanged;
        }
    }
}
