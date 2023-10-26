using EBooking.EntityFramework.DataAccess;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EBooking.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;
        private readonly SettingsStore settingsStore;

        private readonly SettingsService settingsService;
        private readonly NavigationService navigateToLoginViewModel;
        private readonly NavigationService navigateToSettingsViewModel;
        private readonly string _connectionString;
        public App()
        {
            navigationStore = new NavigationStore();
            settingsStore = new SettingsStore();
            settingsService = new SettingsService(settingsStore);
            navigateToLoginViewModel = new NavigationService(navigationStore, CreateLoginViewModel);
            navigateToSettingsViewModel = new NavigationService(navigationStore, CreateSettingsViewModel);
            _connectionString = settingsService.LoadConnectionString() ?? "";
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            SQLiteDAOFactory daoFactory = new SQLiteDAOFactory(_connectionString);
            // Loading necessary stores
            settingsStore.LoadSettings();
            // Creating initial view model of main window
            navigationStore.CurrentViewModel = CreateSettingsViewModel();

            // Initializing main window
            MainWindow = new MainWindow()
            {
                DataContext = CreateMainViewModel()
            };
            // Displaying main window
            MainWindow.Show();
        }

        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(navigateToSettingsViewModel, navigateToLoginViewModel);
        }

        private MainViewModel CreateMainViewModel()
        {
            return new MainViewModel(navigationStore, settingsService, CreateMenuViewModel());
        }

        private LoginViewModel CreateLoginViewModel()
        {
            return new LoginViewModel();
        }

        private SettingsViewModel CreateSettingsViewModel()
        {
            return new SettingsViewModel(settingsStore, settingsService);
        }
    }
}
