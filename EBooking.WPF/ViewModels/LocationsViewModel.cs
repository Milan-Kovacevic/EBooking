using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EBooking.Domain.DTOs;
using EBooking.WPF.Messages;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace EBooking.WPF.ViewModels
{
    public partial class LocationsViewModel : ObservableObject, IViewModelBase, IRecipient<DeleteSelectedDialogResultMessage>
    {
        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value != null && value != string.Empty)
                return;
            SearchLocations();
        }

        private readonly ObservableCollection<LocationItemViewModel> _locations;
        public ICollectionView Locations { get; }
        public bool? IsAllItemsSelected
        {
            get
            {
                var selected = _locations.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value);
                    OnPropertyChanged();
                }
            }
        }

        private readonly LocationsStore _locationsStore;
        private readonly LocationsService _locationsService;
        private readonly DialogHostService _dialogHostService;

        public LocationsViewModel(LocationsStore locationsStore, LocationsService locationsService, DialogHostService dialogHostService)
        {
            _locationsService = locationsService;
            _dialogHostService = dialogHostService;
            _locationsStore = locationsStore;
            _locationsStore.LocationAdded += OnLocationAdded;
            _locationsStore.LocationUpdated += OnLocationUpdated;
            _locationsStore.LocationDeleted += OnLocationDeleted;

            searchText = string.Empty;

            _locations = new ObservableCollection<LocationItemViewModel>(locationsStore.Locations.Select(x => Mapper.Map(x).ToANew<LocationItemViewModel>()));
            Locations = CollectionViewSource.GetDefaultView(_locations);
            Locations.Filter = FilterLocations;
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Dispose()
        {
            _locationsStore.LocationAdded -= OnLocationAdded;
            _locationsStore.LocationUpdated -= OnLocationUpdated;
            _locationsStore.LocationDeleted -= OnLocationDeleted;
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        #region Locations CRUD Commands
        private void OnLocationAdded(Location location)
        {
            _locations.Add(Mapper.Map(location).ToANew<LocationItemViewModel>());
        }
        private void OnLocationUpdated(Location location)
        {
            var locationItemVm = _locations.FirstOrDefault(f => f.LocationId == location.LocationId);
            Mapper.Map(location).Over(locationItemVm);
        }
        private void OnLocationDeleted(int id)
        {
            var locationItemVm = _locations.FirstOrDefault(f => f.LocationId == id);

            if (locationItemVm is not null)
                _locations.Remove(locationItemVm);
        }

        [RelayCommand]
        public void SearchLocations()
        {
            try
            {
                Locations.Refresh();
            }
            catch
            { }
        }

        [RelayCommand]
        public void AddLocation()
        {
            _dialogHostService.OpenLocationAddDialog();
        }

        [RelayCommand]
        public void DeleteLocation(object param)
        {
            if (param is not LocationItemViewModel vm)
                return;
            _locationsService.SetSelectedLocation(vm.LocationId);
            _dialogHostService.OpenLocationDeleteDialog();
        }

        [RelayCommand]
        public void DeleteSelectedLocations()
        {
            _dialogHostService.OpenConfirmMultiDeleteDialog();
        }

        public async void Receive(DeleteSelectedDialogResultMessage message)
        {
            if (message.DialogResult == false)
                return;
            List<Task> tasks = new List<Task>();
            for (int i = _locations.Count - 1; i >= 0; i--)
            {
                if (_locations[i].IsSelected)
                {
                    tasks.Add(_locationsService.DeleteLocation(_locations[i].LocationId));
                }
            }
            await Task.WhenAll(tasks);
            IsAllItemsSelected = false;
        }

        [RelayCommand]
        public void EditLocation(object param)
        {
            if (param is not LocationItemViewModel vm)
                return;
            _locationsService.SetSelectedLocation(vm.LocationId);
            _dialogHostService.OpenLocationEditDialog();
        }

        #endregion
        #region ViewModel Helper Functions
        private bool FilterLocations(object obj)
        {
            if (obj is null || obj is not LocationItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.Country.ToLower().Contains(SearchText.ToLower()) || vm.City.ToLower().Contains(SearchText.ToLower());
        }

        private void SelectAll(bool select)
        {
            foreach (var item in Locations)
            {
                if (item is LocationItemViewModel vm)
                {
                    if (FilterLocations(vm))
                        vm.IsSelected = select;
                }
            }
        }

        #endregion
    }
}
