using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
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
        private static readonly string USER_MANUAL_FILE_NAME_KEY = "UserManualBaseFileName";
        private static readonly string USER_MANUAL_FILE_EXTENSION_KEY = "UserManualFileExtension";
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
                var userManualFileName = ConfigurationManager.AppSettings[USER_MANUAL_FILE_NAME_KEY];
                var userManualFileExtension = ConfigurationManager.AppSettings[USER_MANUAL_FILE_EXTENSION_KEY];
                if (userManualFileName is null || userManualFileExtension is null)
                    return;
                var exactFileName = $"{userManualFileName} {_settingsStore.CurrentSettings.LanguageCode}.{userManualFileExtension}";
                var filePath = Path.Combine(".", "Resources", exactFileName);

                if (!Path.Exists(filePath))
                {
                    exactFileName = $"{userManualFileName} {LanguageProvider.Instance.Languages.ElementAt(0).Key}.{userManualFileExtension}";
                    filePath = Path.Combine(".", "Resources", exactFileName);
                }

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true,
                };
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private readonly UserStore _userStore;
        private readonly SettingsStore _settingsStore;
        private readonly NavigationService _navigateToRegisterViewModel;
        private readonly NavigationService _navigateToLoginViewModel;

        public LandingViewModel(UserStore userStore, SettingsStore settingsStore, NavigationService navigateToRegisterViewModel, NavigationService navigateToLoginViewModel)
        {
            _userStore = userStore;
            _settingsStore = settingsStore;
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
