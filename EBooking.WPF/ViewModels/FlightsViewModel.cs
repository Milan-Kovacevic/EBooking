using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.ItemViewModels;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.ViewModels
{
    public partial class FlightsViewModel : ObservableObject, IViewModelBase
    {
        private ObservableCollection<FlightItemViewModel> _flights;
        public ICollectionView Flights { get; }

        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value != null && value != string.Empty)
                return;
            SearchFlights();
        }
        [ObservableProperty]
        private bool isFilterSelected;
        [ObservableProperty]
        private bool isAdmin;

        private readonly UserStore _userStore;
        private readonly FlightStore _flightStore;
        private readonly FlightService _flightService;
        private readonly DialogHostService _dialogHostService;

        public FlightsViewModel(UserStore userStore, FlightStore flightStore, FlightService flightService, DialogHostService dialogHostService)
        {
            _userStore = userStore;
            _flightStore = flightStore;
            _flightService = flightService;
            _dialogHostService = dialogHostService;

            searchText = string.Empty;
            isFilterSelected = false;
            isAdmin = userStore.IsAdmin;

            _flightStore.FlightLoaded += OnFlightLoaded;
            _flightStore.FlightAdded += OnFlightAdded;
            _flightStore.FlightUpdated += OnFlightUpdated;
            _flightStore.FlightDeleted += OnFlightDeleted;
            _flights = new ObservableCollection<FlightItemViewModel>();
            Flights = CollectionViewSource.GetDefaultView(_flights);
            Flights.Filter = FilterFlights;
            LoadFlights();
        }

        public async void LoadFlights()
        {
            await _flightStore.Load();
        }

        public void Dispose()
        {
            _flightStore.FlightLoaded -= OnFlightLoaded;
            _flightStore.FlightAdded -= OnFlightAdded;
            _flightStore.FlightUpdated -= OnFlightUpdated;
            _flightStore.FlightDeleted -= OnFlightDeleted;
        }

        #region Flights CRUD Commands
        private void OnFlightLoaded()
        {
            _flights.Clear();
            foreach (var item in _flightStore.Flights)
                AddNewFlightItem(item);
        }

        private void OnFlightAdded(Flight flight)
        {
            AddNewFlightItem(flight);
        }

        private void AddNewFlightItem(Flight flight)
        {
            var flightItem = new FlightItemViewModel(_flightService, _dialogHostService);
            Mapper.Map(flight).Over(flightItem);
            flightItem.IsOwner = _userStore.CurrentUser?.UserId == flight.UserId;
            _flights.Add(flightItem);
        }

        private void OnFlightUpdated(Flight flight)
        {
            var flightItemVm = _flights.FirstOrDefault(f => f.FlightId == flight.FlightId);
            Mapper.Map(flight).Over(flightItemVm);
        }

        private void OnFlightDeleted(int id)
        {
            var flightItemVm = _flights.FirstOrDefault(f => f.FlightId == id);
            if (flightItemVm is not null)
                _flights.Remove(flightItemVm);
        }

        [RelayCommand]
        public void SearchFlights()
        {
            try
            {
                Flights.Refresh();
            }
            catch
            { }
        }

        [RelayCommand]
        public void AddFlight()
        {
            _dialogHostService.OpenFlightAddDialog();
        }

        [RelayCommand]
        public async Task FilterFlights()
        {
            // TODO
            await Task.Delay(200);
        }
        #endregion

        private bool FilterFlights(object obj)
        {
            if (obj is null || obj is not FlightItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.AvioCompanyName.ToLower().Contains(SearchText.ToLower());
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.FLIGHTS);
    }
}
