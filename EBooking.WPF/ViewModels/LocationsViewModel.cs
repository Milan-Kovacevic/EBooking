using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.ViewModels
{
    public partial class LocationsViewModel : ObservableObject, IViewModelBase
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

        private readonly DialogHostService _dialogHostService;

        public LocationsViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
            searchText = string.Empty;
            _locations = new ObservableCollection<LocationItemViewModel>()
            {
                new LocationItemViewModel(){ LocationId = 1, Country="Serbia", City="Belgrade" },
                new LocationItemViewModel(){ LocationId = 2, Country="Serbia", City="Novi Sad" },
                new LocationItemViewModel(){ LocationId = 3, Country="Bosnia and Herzegovina", City="Banja Luka" },
                new LocationItemViewModel(){ LocationId = 4, Country="Serbia", City="Kragujevac" },
                new LocationItemViewModel(){ LocationId = 5, Country="Bosnia and Herzegovina", City="Gradiska" },
                new LocationItemViewModel(){ LocationId = 6, Country="Bosnia and Herzegovina", City="Sarajevo" },
                new LocationItemViewModel(){ LocationId = 7, Country="Montenegro", City="Budva" },
                new LocationItemViewModel(){ LocationId = 8, Country="Croatia", City="Zagreb" },
            };
            Locations = CollectionViewSource.GetDefaultView(_locations);
            Locations.Filter = FilterLocations;
        }

        public void Dispose() { }

        #region Locations CRUD Commands

        [RelayCommand]
        public void SearchLocations()
        {
            try
            {
                Locations.Refresh();
            }
            catch
            {

            }
        }

        [RelayCommand]
        public async Task AddLocation()
        {
            await _dialogHostService.ShowAddLocationDialog((locationVM) =>
            {
                _locations.Add(new LocationItemViewModel() { City = locationVM.CityName, Country = locationVM.CountryName, LocationId = _locations.Count + 1 });
                _dialogHostService.CloseDialogHost();
            });
        }

        [RelayCommand]
        public async Task DeleteLocation(object param)
        {
            if (param is not LocationItemViewModel vm)
                return;

            await _dialogHostService.ShowConfirmDeleteDialog(() =>
            {
                _locations.Remove(vm);
                _dialogHostService.CloseDialogHost();
            });
        }

        [RelayCommand]
        public async Task DeleteSelectedLocations()
        {
            await _dialogHostService.ShowConfirmDeleteDialog(() =>
            {
                for (int i = _locations.Count - 1; i >= 0; i--)
                    if (_locations[i].IsSelected)
                        _locations.RemoveAt(i);
                _dialogHostService.CloseDialogHost();
                IsAllItemsSelected = false;
            });
        }

        [RelayCommand]
        public async Task EditLocation(object param)
        {
            if (param is not LocationItemViewModel vm)
                return;

            await _dialogHostService.ShowEditLocationDialog((locationVM) =>
            {
                var element = _locations.First(x => x.LocationId == vm.LocationId);
                element.Country = locationVM.CountryName;
                element.City = locationVM.CityName;
                _dialogHostService.CloseDialogHost();
            }, vm);
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
