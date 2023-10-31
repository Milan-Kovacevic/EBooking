using EBooking.Domain.DAOs;
using EBooking.EntityFramework.DataAccess;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.ViewModels;
using MaterialDesignThemes.Wpf;
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
        private readonly MessageQueueStore messageQueueStore;
        private readonly SettingsStore settingsStore;
        private readonly UserStore userStore;

        private readonly UserService userService;
        private readonly SettingsService settingsService;
        private readonly MessageQueueService messageQueueService;
        private readonly NavigationService navigateToLoginViewModel;
        private readonly NavigationService navigateToRegisterViewModel;
        private readonly NavigationService navigateToSettingsViewModel;
        private readonly NavigationService navigateToLandingViewModel;
        private readonly NavigationService navigateToCodebookViewModel;
        private readonly DialogHostService dialogHostService;
        private readonly IGenericDAOFactory daoFactory;
        private readonly string _connectionString;

        public App()
        {
            navigationStore = new NavigationStore();
            userStore = new UserStore();
            messageQueueStore = new MessageQueueStore(new SnackbarMessageQueue(TimeSpan.FromSeconds(2)));
            settingsStore = new SettingsStore();
            userService = new UserService(userStore);
            settingsService = new SettingsService(settingsStore);
            messageQueueService = new MessageQueueService(messageQueueStore);
            navigateToLoginViewModel = new NavigationService(navigationStore, CreateLoginViewModel);
            navigateToRegisterViewModel = new NavigationService(navigationStore, CreateRegisterViewModel);
            navigateToSettingsViewModel = new NavigationService(navigationStore, CreateSettingsViewModel);
            navigateToLandingViewModel = new NavigationService(navigationStore, CreateLandingViewModel);
            navigateToCodebookViewModel = new NavigationService(navigationStore, CreateCodebookViewModel);
            dialogHostService = new DialogHostService();

            _connectionString = settingsService.LoadConnectionString() ?? "";
            daoFactory = new SQLiteDAOFactory(_connectionString);
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // Loading necessary stores
            settingsStore.LoadSettings();
            // Initializing main window
            MainWindow = new MainWindow()
            {
                DataContext = CreateMainViewModel()
            };
            // Creating initial view model of main window
            navigationStore.CurrentViewModel = CreateLandingViewModel();
            // Displaying main window
            MainWindow.Show();
            messageQueueService.Enqueue("Welcome back!");
        }


        // TODO: ADD DEPENDENCY INJECTION
        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(userStore, navigateToSettingsViewModel, navigateToLoginViewModel, navigateToRegisterViewModel, navigateToCodebookViewModel);
        }

        private LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel(userStore, navigateToRegisterViewModel, navigateToLoginViewModel);
        }

        private MainViewModel CreateMainViewModel()
        {
            return new MainViewModel(messageQueueStore, navigationStore, userStore, userService, settingsService, CreateMenuViewModel(), navigateToLandingViewModel, dialogHostService);
        }

        private LoginViewModel CreateLoginViewModel()
        {
            return new LoginViewModel(userService, navigateToRegisterViewModel, navigateToLandingViewModel);
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(messageQueueService);
        }

        private SettingsViewModel CreateSettingsViewModel()
        {
            return new SettingsViewModel(messageQueueService, settingsStore, settingsService);
        }

        private CodebookViewModel CreateCodebookViewModel()
        {
            return new CodebookViewModel(CreateLocationsViewModel, CreateUnitFeaturesViewModel);
        }

        private LocationsViewModel CreateLocationsViewModel()
        {
            return new LocationsViewModel(dialogHostService);
        }

        private UnitFeaturesViewModel CreateUnitFeaturesViewModel()
        {
            return new UnitFeaturesViewModel(dialogHostService);
        }

    }
}
