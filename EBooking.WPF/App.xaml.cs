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
using static EBooking.WPF.Utility.LanguageTranslator;

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
        private readonly AccommodationUnitStore accommodationUnitStore;
        private readonly UnitReservationStore unitReservationStore;
        private readonly FlightStore flightStore;
        private readonly TripReservationStore tripReservationStore;

        private readonly UserService userService;
        private readonly LocationsService locationsService;
        private readonly UnitFeaturesService unitFeaturesService;
        private readonly AccommodationService accommodationService;
        private readonly AccommodationUnitService accommodationUnitService;
        private readonly UnitReservationService unitReservationService;
        private readonly FlightService flightService;
        private readonly TripReservationService tripReservationService;
        private readonly SettingsService settingsService;
        private readonly MessageQueueService messageQueueService;

        private readonly NavigationService navigateToLoginViewModel;
        private readonly NavigationService navigateToRegisterViewModel;
        private readonly NavigationService navigateToSettingsViewModel;
        private readonly NavigationService navigateToLandingViewModel;
        private readonly NavigationService navigateToAccommodationsViewModel;
        private readonly NavigationService navigateToAccommodationDetailsViewModel;
        private readonly NavigationService navigateToFlightsViewModel;
        private readonly NavigationService navigateToCodebookViewModel;
        private readonly NavigationService navigateToReservationsViewModel;
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
        private readonly DialogNavigationService navigateToAccommodationUnitDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToAccommodationUnitAddDialogViewModel;
        private readonly DialogNavigationService navigateToAccommodationUnitEditDialogViewModel;
        private readonly DialogNavigationService navigateToUnitReservationAddDialogViewModel;
        private readonly DialogNavigationService navigateToUnitReservationEditDialogViewModel;
        private readonly DialogNavigationService navigateToUnitReservationDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToSaveSettingsDialogViewModel;
        private readonly DialogNavigationService navigateToFlightAddDialogViewModel;
        private readonly DialogNavigationService navigateToFlightEditDialogViewModel;
        private readonly DialogNavigationService navigateToFlightDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToChangePasswordDialogViewModel;
        private readonly DialogNavigationService navigateToChangeUserInfoDialogViewModel;
        private readonly DialogNavigationService navigateToTripReservationAddDialogViewModel;
        private readonly DialogNavigationService navigateToTripReservationEditDialogViewModel;
        private readonly DialogNavigationService navigateToTripReservationDeleteDialogViewModel;
        private readonly DialogNavigationService navigateToTripReservationDetailsDialogViewModel;
        private readonly DialogNavigationService navigateToUnitReservationDetailsDialogViewModel;
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
            accommodationUnitStore = new AccommodationUnitStore(daoFactory.AccommodationUnitDao);
            unitReservationStore = new UnitReservationStore(daoFactory.UnitReservationDao);
            tripReservationStore = new TripReservationStore(daoFactory.TripReservationDao);
            flightStore = new FlightStore(daoFactory.FlightDao);
            messageQueueStore = new MessageQueueStore(new SnackbarMessageQueue(TimeSpan.FromSeconds(2)));
            userService = new UserService(userStore);
            locationsService = new LocationsService(locationsStore);
            unitFeaturesService = new UnitFeaturesService(unitFeaturesStore);
            accommodationService = new AccommodationService(accommodationStore);
            accommodationUnitService = new AccommodationUnitService(accommodationUnitStore);
            unitReservationService = new UnitReservationService(unitReservationStore);
            tripReservationService = new TripReservationService(tripReservationStore);
            flightService = new FlightService(flightStore);
            messageQueueService = new MessageQueueService(messageQueueStore);
            navigateToLoginViewModel = new NavigationService(navigationStore, CreateLoginViewModel);
            navigateToRegisterViewModel = new NavigationService(navigationStore, CreateRegisterViewModel);
            navigateToSettingsViewModel = new NavigationService(navigationStore, CreateSettingsViewModel);
            navigateToLandingViewModel = new NavigationService(navigationStore, CreateLandingViewModel);
            navigateToAccommodationsViewModel = new NavigationService(navigationStore, CreateAccommodationsViewModel);
            navigateToAccommodationDetailsViewModel = new NavigationService(navigationStore, CreateAccommodationDetailsViewModel);
            navigateToFlightsViewModel = new NavigationService(navigationStore, CreateFlightsViewModel);
            navigateToCodebookViewModel = new NavigationService(navigationStore, CreateCodebookViewModel);
            navigateToReservationsViewModel = new NavigationService(navigationStore, CreateReservationsViewModel);
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
            navigateToAccommodationUnitDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateAccommodationUnitDeleteDialogViewModel);
            navigateToAccommodationUnitAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateAccommodationUnitAddDialogViewModel);
            navigateToAccommodationUnitEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateAccommodationUnitEditDialogViewModel);
            navigateToUnitReservationAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitReservationAddDialogViewModel);
            navigateToUnitReservationEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitReservationEditDialogViewModel);
            navigateToUnitReservationDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitReservationDeleteDialogViewModel);
            navigateToSaveSettingsDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateSaveSettingsDialogViewModel);
            navigateToFlightAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateFlightAddDialogViewModel);
            navigateToFlightEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateFlightEditDialogViewModel);
            navigateToFlightDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateFlightDeleteDialogViewModel);
            navigateToChangePasswordDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateChangePasswordDialogViewModel);
            navigateToChangeUserInfoDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateChangeUserInfoDialogViewModel);
            navigateToTripReservationAddDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateTripReservationAddDialogViewModel);
            navigateToTripReservationEditDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateTripReservationEditDialogViewModel);
            navigateToTripReservationDeleteDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateTripReservationDeleteDialogViewModel);
            navigateToTripReservationDetailsDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateTripReservationDetailsDialogViewModel);
            navigateToUnitReservationDetailsDialogViewModel = new DialogNavigationService(dialogNavigationStore, CreateUnitReservationDetailsDialogViewModel);
            dialogHostService = new DialogHostService(navigateToExitApplicationDialogViewModel, navigateToLocationDeleteDialogViewModel, navigateToLocationAddDialogViewModel, navigateToLocationEditDialogViewModel, navigateToUnitFeatureDeleteDialogViewModel, navigateToUnitFeatureAddDialogViewModel, navigateToUnitFeatureEditDialogViewModel, navigateToMultiDeleteDialogViewModel, navigateToAccommodationAddDialogViewModel, navigateToAccommodationEditDialogViewModel, navigateToAccommodationDeleteDialogViewModel, navigateToAccommodationUnitAddDialogViewModel, navigateToAccommodationUnitDeleteDialogViewModel, navigateToAccommodationUnitEditDialogViewModel, navigateToUnitReservationAddDialogViewModel, navigateToUnitReservationEditDialogViewModel, navigateToUnitReservationDeleteDialogViewModel, navigateToSaveSettingsDialogViewModel, navigateToFlightAddDialogViewModel, navigateToFlightEditDialogViewModel, navigateToFlightDeleteDialogViewModel, navigateToChangePasswordDialogViewModel, navigateToTripReservationAddDialogViewModel, navigateToTripReservationEditDialogViewModel, navigateToTripReservationDeleteDialogViewModel, navigateToChangeUserInfoDialogViewModel, navigateToTripReservationDetailsDialogViewModel, navigateToUnitReservationDetailsDialogViewModel);
        }

        private async void InitStores()
        {
            settingsStore.LoadSettings();
            await userStore.Load();
            await locationsStore.Load();
            await unitFeaturesStore.Load();
            await accommodationStore.Load();
            await flightStore.Load();
            await unitReservationStore.Load();
            await tripReservationStore.Load();
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
            messageQueueService.Enqueue(MessageType.WELCOME_MESSAGE);
        }

        // TODO: ADD DEPENDENCY INJECTION
        #region ViewModels Instantiation
        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(userStore, navigateToSettingsViewModel, navigateToLoginViewModel, navigateToRegisterViewModel, navigateToCodebookViewModel, navigateToAccommodationsViewModel, navigateToFlightsViewModel, navigateToReservationsViewModel);
        }

        private LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel(userStore, settingsStore, navigateToRegisterViewModel, navigateToLoginViewModel);
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
            return new SettingsViewModel(messageQueueService, settingsStore, settingsService, dialogHostService, userStore);
        }

        private AccommodationsViewModel CreateAccommodationsViewModel()
        {
            return new AccommodationsViewModel(accommodationStore, accommodationService, dialogHostService, userStore, navigateToAccommodationDetailsViewModel, messageQueueService);
        }

        private AccommodationDetailsViewModel CreateAccommodationDetailsViewModel()
        {
            return new AccommodationDetailsViewModel(accommodationStore, navigateToAccommodationsViewModel, CreateAccommodationUnitsViewModel(), CreateUnitReservationsViewModel());
        }

        private AccommodationUnitsViewModel CreateAccommodationUnitsViewModel()
        {
            return new AccommodationUnitsViewModel(accommodationStore, accommodationUnitStore, userStore, dialogHostService, accommodationUnitService, messageQueueService);
        }

        private UnitReservationsViewModel CreateUnitReservationsViewModel()
        {
            return new UnitReservationsViewModel(accommodationStore, unitReservationStore, userStore, unitReservationService, dialogHostService, messageQueueService);
        }

        private FlightsViewModel CreateFlightsViewModel()
        {
            return new FlightsViewModel(userStore, flightStore, flightService, dialogHostService);
        }

        private CodebookViewModel CreateCodebookViewModel()
        {
            return new CodebookViewModel(CreateLocationsViewModel(), CreateUnitFeaturesViewModel());
        }

        private LocationsViewModel CreateLocationsViewModel()
        {
            return new LocationsViewModel(locationsStore, locationsService, dialogHostService);
        }

        private UnitFeaturesViewModel CreateUnitFeaturesViewModel()
        {
            return new UnitFeaturesViewModel(unitFeaturesStore, unitFeaturesService, dialogHostService);
        }

        private ReservationsViewModel CreateReservationsViewModel()
        {
            return new ReservationsViewModel(CreateAccommodationReservationsViewModel(), CreateTripReservationsViewModel());
        }

        private AccommodationReservationsViewModel CreateAccommodationReservationsViewModel()
        {
            return new AccommodationReservationsViewModel(unitReservationStore, userStore, unitReservationService, dialogHostService, messageQueueService);
        }

        private TripReservationsViewModel CreateTripReservationsViewModel()
        {
            return new TripReservationsViewModel(tripReservationStore, tripReservationService, userStore, dialogHostService, messageQueueService);
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
        public AccommodationUnitAddDialogViewModel CreateAccommodationUnitAddDialogViewModel()
        {
            return new AccommodationUnitAddDialogViewModel(unitFeaturesStore, accommodationUnitService, dialogHostService);
        }
        public AccommodationUnitDeleteDialogViewModel CreateAccommodationUnitDeleteDialogViewModel()
        {
            return new AccommodationUnitDeleteDialogViewModel(accommodationUnitService, dialogHostService);
        }
        public AccommodationUnitEditDialogViewModel CreateAccommodationUnitEditDialogViewModel()
        {
            return new AccommodationUnitEditDialogViewModel(unitFeaturesStore, accommodationUnitService, dialogHostService);
        }
        public UnitReservationAddDialogViewModel CreateUnitReservationAddDialogViewModel()
        {
            return new UnitReservationAddDialogViewModel(unitReservationService, userStore, accommodationUnitStore, dialogHostService);
        }
        public UnitReservationEditDialogViewModel CreateUnitReservationEditDialogViewModel()
        {
            return new UnitReservationEditDialogViewModel(unitReservationStore, unitReservationService, userStore, dialogHostService);
        }
        public UnitReservationDeleteDialogViewModel CreateUnitReservationDeleteDialogViewModel()
        {
            return new UnitReservationDeleteDialogViewModel(unitReservationService, dialogHostService);
        }
        public SaveSettingsDialogViewModel CreateSaveSettingsDialogViewModel()
        {
            return new SaveSettingsDialogViewModel();
        }
        public FlightAddDialogViewModel CreateFlightAddDialogViewModel()
        {
            return new FlightAddDialogViewModel(locationsStore, flightService, userStore, dialogHostService);
        }
        public FlightEditDialogViewModel CreateFlightEditDialogViewModel()
        {
            return new FlightEditDialogViewModel(locationsStore, flightService, userStore, dialogHostService);
        }
        public FlightDeleteDialogViewModel CreateFlightDeleteDialogViewModel()
        {
            return new FlightDeleteDialogViewModel(flightService, dialogHostService);
        }
        public ChangePasswordDialogViewModel CreateChangePasswordDialogViewModel()
        {
            return new ChangePasswordDialogViewModel(userService, dialogHostService, messageQueueService);
        }
        public ChangeUserInfoDialogViewModel CreateChangeUserInfoDialogViewModel()
        {
            return new ChangeUserInfoDialogViewModel(userStore, userService, dialogHostService, messageQueueService);
        }
        public TripReservationAddDialogViewModel CreateTripReservationAddDialogViewModel()
        {
            return new TripReservationAddDialogViewModel(flightStore, tripReservationService, dialogHostService, userStore);
        }
        public TripReservationEditDialogViewModel CreateTripReservationEditDialogViewModel()
        {
            return new TripReservationEditDialogViewModel(flightStore, tripReservationService, dialogHostService, userStore);
        }
        public TripReservationDeleteDialogViewModel CreateTripReservationDeleteDialogViewModel()
        {
            return new TripReservationDeleteDialogViewModel(tripReservationService, dialogHostService);
        }
        public TripReservationDetailsDialogViewModel CreateTripReservationDetailsDialogViewModel()
        {
            return new TripReservationDetailsDialogViewModel(tripReservationService);
        }
        public UnitReservationDetailsDialogViewModel CreateUnitReservationDetailsDialogViewModel()
        {
            return new UnitReservationDetailsDialogViewModel(unitReservationService);
        }
        #endregion
    }
}
