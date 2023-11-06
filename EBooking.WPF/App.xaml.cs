using EBooking.Domain.DAOs;
using EBooking.EntityFramework.DataAccess;
using EBooking.WPF.Dialogs.ViewModels;
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
        private readonly DialogNavigationStore dialogNavigationStore;
        private readonly MessageQueueStore messageQueueStore;
        private readonly SettingsStore settingsStore;
        private readonly UserStore userStore;
        private readonly LocationsStore locationsStore;
        private readonly UnitFeaturesStore unitFeaturesStore;
        private readonly AccommodationStore accommodationStore;

        private readonly UserService userService;
        private readonly LocationsService locationsService;
        private readonly UnitFeaturesService unitFeaturesService;
        private readonly AccommodationService accommodationService;
        private readonly SettingsService settingsService;
        private readonly MessageQueueService messageQueueService;

        private readonly NavigationService navigateToLoginViewModel;
        private readonly NavigationService navigateToRegisterViewModel;
        private readonly NavigationService navigateToSettingsViewModel;
        private readonly NavigationService navigateToLandingViewModel;
        private readonly NavigationService navigateToAccommodationsViewModel;
        private readonly NavigationService navigateToAccommodationUnitsViewModel;
        private readonly NavigationService navigateToFlightsViewModel;
        private readonly NavigationService navigateToCodebookViewModel;
        private readonly DialogHostService dialogHostService;
        private readonly DialogNavigationService navigateToExitApplicationDialogViewModel;
        private readonly DialogNavigationService navigateToLocationDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToLocationAddDialogViewModel;
        private readonly DialogNavigationService navigateToLocationEditDialogViewModel;
        private readonly DialogNavigationService navigateToUnitFeatureDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToUnitFeatureAddDialogViewModel;
        private readonly DialogNavigationService navigateToUnitFeatureEditDialogViewModel;
        private readonly DialogNavigationService navigateToMultiDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToAccommodationDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToAccommodationAddDialogViewModel;
        private readonly DialogNavigationService navigateToAccommodationEditDialogViewModel;
        private readonly IGenericDAOFactory daoFactory;
        private readonly string _connectionString;

        public App()
        {
            settingsStore = new SettingsStore();
            settingsService = new SettingsService(settingsStore);
            _connectionString = settingsService.LoadConnectionString() ?? "";
            daoFactory = new SQLiteDAOFactory(_connectionString);

            navigationStore = new NavigationStore();
            dialogNavigationStore = new DialogNavigationStore();
            userStore = new UserStore(daoFactory.UserDao);
            locationsStore = new LocationsStore(daoFactory.LocationDao);
            unitFeaturesStore = new UnitFeaturesStore(daoFactory.UnitFeatureDao);
            accommodationStore = new AccommodationStore(daoFactory.AccommodationDao);
            messageQueueStore = new MessageQueueStore(new SnackbarMessageQueue(TimeSpan.FromSeconds(2)));
            userService = new UserService(userStore);
            locationsService = new LocationsService(locationsStore);
            unitFeaturesService = new UnitFeaturesService(unitFeaturesStore);
            accommodationService = new AccommodationService(accommodationStore);
            messageQueueService = new MessageQueueService(messageQueueStore);
            navigateToLoginViewModel = new NavigationService(navigationStore, CreateLoginViewModel);
            navigateToRegisterViewModel = new NavigationService(navigationStore, CreateRegisterViewModel);
            navigateToSettingsViewModel = new NavigationService(navigationStore, CreateSettingsViewModel);
            navigateToLandingViewModel = new NavigationService(navigationStore, CreateLandingViewModel);
            navigateToAccommodationsViewModel = new NavigationService(navigationStore, CreateAccommodationsViewModel);
            navigateToAccommodationUnitsViewModel = new NavigationService(navigationStore, CreateAccommodationUnitsViewModel);
            navigateToFlightsViewModel = new NavigationService(navigationStore, CreateFlightsViewModel);
            navigateToCodebookViewModel = new NavigationService(navigationStore, CreateCodebookViewModel);
            navigateToExitApplicationDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateExitApplicationDialogViewModel);
            navigateToLocationDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateLocationDeleteDialogViewModel);
            navigateToLocationAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateLocationAddDialogViewModel);
            navigateToLocationEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateLocationEditDialogViewModel);
            navigateToUnitFeatureDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitFeatureDeleteDialogViewModel);
            navigateToUnitFeatureAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitFeatureAddDialogViewModel);
            navigateToUnitFeatureEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitFeatureEditDialogViewModel);
            navigateToMultiDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateMultiDeleteDialogViewModel);
            navigateToAccommodationDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateAccommodationDeleteDialogViewModel);
            navigateToAccommodationAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateAccommodationAddDialogViewModel);
            navigateToAccommodationEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateAccommodationEditDialogViewModel);
            dialogHostService = new DialogHostService(navigateToExitApplicationDialogViewModel, navigateToLocationDeleteDialogViewModel, navigateToLocationAddDialogViewModel, navigateToLocationEditDialogViewModel, navigateToUnitFeatureDeleteDialogViewModel, navigateToUnitFeatureAddDialogViewModel, navigateToUnitFeatureEditDialogViewModel, navigateToMultiDeleteDialogViewModel, navigateToAccommodationAddDialogViewModel, navigateToAccommodationEditDialogViewModel, navigateToAccommodationDeleteDialogViewModel);
        }

        private async void InitStores()
        {
            settingsStore.LoadSettings();
            await userStore.Load();
            await locationsStore.Load();
            await unitFeaturesStore.Load();
            await accommodationStore.Load();
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
        #region ViewModels Instantiation
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
            return new MainViewModel(messageQueueStore, navigationStore, dialogNavigationStore, userStore, userService, settingsService, CreateMenuViewModel(), navigateToLandingViewModel, dialogHostService);
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
            return new AccommodationsViewModel(accommodationStore, accommodationService, dialogHostService, userStore, navigateToAccommodationUnitsViewModel);
        }

        private AccommodationUnitsViewModel CreateAccommodationUnitsViewModel()
        {
            return new AccommodationUnitsViewModel(accommodationStore, navigateToAccommodationsViewModel);
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
        #endregion

        #region DialogViewModels Instantiation

        public ExitApplicationDialogViewModel CreateExitApplicationDialogViewModel()
        {
            return new ExitApplicationDialogViewModel(dialogHostService);
        }
        public LocationAddDialogViewModel CreateLocationAddDialogViewModel()
        {
            return new LocationAddDialogViewModel(locationsService, dialogHostService);
        }
        public LocationDeleteDialogViewModel CreateLocationDeleteDialogViewModel()
        {
            return new LocationDeleteDialogViewModel(locationsService, dialogHostService);
        }
        public LocationEditDialogViewModel CreateLocationEditDialogViewModel()
        {
            return new LocationEditDialogViewModel(locationsService, dialogHostService);
        }
        public UnitFeatureDeleteDialogViewModel CreateUnitFeatureDeleteDialogViewModel()
        {
            return new UnitFeatureDeleteDialogViewModel(unitFeaturesService, dialogHostService);
        }
        public UnitFeatureAddDialogViewModel CreateUnitFeatureAddDialogViewModel()
        {
            return new UnitFeatureAddDialogViewModel(unitFeaturesService, dialogHostService);
        }
        public UnitFeatureEditDialogViewModel CreateUnitFeatureEditDialogViewModel()
        {
            return new UnitFeatureEditDialogViewModel(unitFeaturesService, dialogHostService);
        }
        public MultiDeleteDialogViewModel CreateMultiDeleteDialogViewModel()
        {
            return new MultiDeleteDialogViewModel(dialogHostService);
        }
        public AccommodationDeleteDialogViewModel CreateAccommodationDeleteDialogViewModel()
        {
            return new AccommodationDeleteDialogViewModel(accommodationService, dialogHostService);
        }
        public AccommodationAddDialogViewModel CreateAccommodationAddDialogViewModel()
        {
            return new AccommodationAddDialogViewModel(locationsStore, userStore, accommodationService, dialogHostService);
        }
        public AccommodationEditDialogViewModel CreateAccommodationEditDialogViewModel()
        {
            return new AccommodationEditDialogViewModel(locationsStore, userStore, accommodationService, dialogHostService);
        }
        #endregion
    }
}
