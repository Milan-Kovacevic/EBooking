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
        private readonly LocationsStore locationsStore;
        private readonly UnitFeaturesStore unitFeaturesStore;

        private readonly UserService userService;
        private readonly LocationsService locationsService;
        private readonly UnitFeaturesService unitFeaturesService;
        private readonly SettingsService settingsService;
        private readonly MessageQueueService messageQueueService;
        private readonly NavigationService navigateToLoginViewModel;
        private readonly NavigationService navigateToRegisterViewModel;
        private readonly NavigationService navigateToSettingsViewModel;
        private readonly NavigationService navigateToLandingViewModel;
        private readonly NavigationService navigateToAccommodationsViewModel;
        private readonly NavigationService navigateToFlightsViewModel;
        private readonly NavigationService navigateToCodebookViewModel;
        private readonly DialogHostService dialogHostService;
        private readonly IGenericDAOFactory daoFactory;
        private readonly string _connectionString;

        public App()
        {
            settingsStore = new SettingsStore();
            settingsService = new SettingsService(settingsStore);
            _connectionString = settingsService.LoadConnectionString() ?? "";
            daoFactory = new SQLiteDAOFactory(_connectionString);
           
            navigationStore = new NavigationStore();
            userStore = new UserStore(daoFactory.UserDao);
            locationsStore = new LocationsStore(daoFactory.LocationDao);
            unitFeaturesStore = new UnitFeaturesStore(daoFactory.UnitFeatureDao);
            messageQueueStore = new MessageQueueStore(new SnackbarMessageQueue(TimeSpan.FromSeconds(2)));
            userService = new UserService(userStore);
            locationsService = new LocationsService(locationsStore);
            unitFeaturesService = new UnitFeaturesService(unitFeaturesStore);
            messageQueueService = new MessageQueueService(messageQueueStore);
            navigateToLoginViewModel = new NavigationService(navigationStore, CreateLoginViewModel);
            navigateToRegisterViewModel = new NavigationService(navigationStore, CreateRegisterViewModel);
            navigateToSettingsViewModel = new NavigationService(navigationStore, CreateSettingsViewModel);
            navigateToLandingViewModel = new NavigationService(navigationStore, CreateLandingViewModel);
            navigateToAccommodationsViewModel = new NavigationService(navigationStore, CreateAccommodationsViewModel);
            navigateToFlightsViewModel = new NavigationService(navigationStore, CreateFlightsViewModel);
            navigateToCodebookViewModel = new NavigationService(navigationStore, CreateCodebookViewModel);
            dialogHostService = new DialogHostService(locationsStore);
        }

        private async void InitStores()
        {
            settingsStore.LoadSettings();
            await userStore.Load();
            await locationsStore.Load();
            await unitFeaturesStore.Load();
        }


        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // Loading necessary stores
            InitStores();
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
            return new MenuViewModel(userStore, navigateToSettingsViewModel, navigateToLoginViewModel, navigateToRegisterViewModel, navigateToCodebookViewModel, navigateToAccommodationsViewModel, navigateToFlightsViewModel);
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
            return new LoginViewModel(messageQueueService, userService, navigateToRegisterViewModel, navigateToLandingViewModel);
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(messageQueueService, userService);
        }

        private SettingsViewModel CreateSettingsViewModel()
        {
            return new SettingsViewModel(messageQueueService, settingsStore, settingsService);
        }

        private AccommodationsViewModel CreateAccommodationsViewModel()
        {
            return new AccommodationsViewModel(dialogHostService, userStore);
        }

        private FlightsViewModel CreateFlightsViewModel()
        {
            return new FlightsViewModel();
        }

        private CodebookViewModel CreateCodebookViewModel()
        {
            return new CodebookViewModel(CreateLocationsViewModel, CreateUnitFeaturesViewModel);
        }

        private LocationsViewModel CreateLocationsViewModel()
        {
            return new LocationsViewModel(locationsStore, locationsService, dialogHostService);
        }

        private UnitFeaturesViewModel CreateUnitFeaturesViewModel()
        {
            return new UnitFeaturesViewModel(unitFeaturesStore, unitFeaturesService, dialogHostService);
        }

    }
}
