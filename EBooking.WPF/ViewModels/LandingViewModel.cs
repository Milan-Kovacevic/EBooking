using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBooking.WPF.ViewModels
{
    public partial class LandingViewModel : ObservableObject, IViewModelBase
    {
        private static readonly string DOCUMENTATION_FILE_CONFIGURATION_KEY = "UserManualFileName";
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
            try
            {
                var userManualFileName = ConfigurationManager.AppSettings[DOCUMENTATION_FILE_CONFIGURATION_KEY];
                if (userManualFileName is null)
                    return;
                var filePath = Path.Combine(".", "Resources", userManualFileName);
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true,
                };
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
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
