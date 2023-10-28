using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class LandingViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private bool isLoginEnabled;

        public void NavigateToRegister()
        {
            _navigateToRegisterViewModel.Navigate();
        }

        public void NavigateToLogin()
        {
            _navigateToLoginViewModel.Navigate();
        }

        public void OpenApplicationManual()
        {
            // TODO
        }

        private readonly UserStore _userStore;
        private readonly NavigationService _navigateToRegisterViewModel;
        private readonly NavigationService _navigateToLoginViewModel;

        public LandingViewModel(UserStore userStore, NavigationService navigateToRegisterViewModel, NavigationService navigateToLoginViewModel)
        {
            _userStore = userStore;
            _userStore.CurrentUserChanged += OnCurrentUserChanged;
            _navigateToRegisterViewModel = navigateToRegisterViewModel;
            _navigateToLoginViewModel = navigateToLoginViewModel;
            isLoginEnabled = !userStore.IsLoggedIn;
        }

        private void OnCurrentUserChanged()
        {
            IsLoginEnabled = !_userStore.IsLoggedIn;
        }

        public void Dispose()
        {
            _userStore.CurrentUserChanged -= OnCurrentUserChanged;
        }
    }
}
