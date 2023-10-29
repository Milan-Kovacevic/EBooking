using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Utility;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace EBooking.WPF.ViewModels
{
    public partial class CodebookViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value == null)
            {
                try
                {
                    Locations.Refresh();
                }
                catch
                {
                }
            }
            else if (value == string.Empty)
                Locations.Refresh();
        }

        private ObservableCollection<LocationItemViewModel> _locations;
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

        [RelayCommand]
        private async Task DeleteSelectedLocations()
        {
            var dialogContent = new ConfirmDeleteDialog();
            dialogContent.OnYesCommand = new RelayCommand(() =>
            {
                // TODO
                DialogHost.Close("RootDialog");
            });
            await DialogHost.Show(dialogContent, "RootDialog");
        }

        [RelayCommand]
        private void SearchLocations()
        {
            Locations.Refresh();
        }

        [RelayCommand]
        private async Task AddLocation()
        {
            var onLocationAddAction = (Location location) => 
            {
                _locations.Add(new LocationItemViewModel() { City = location.City, Country = location.Country, LocationId = _locations.Count + 1 });
                DialogHost.Close("RootDialog");
            };
            var dialogContent = new SubmitLocationDialog(onLocationAddAction);
            await DialogHost.Show(dialogContent, "RootDialog");
        }

        [RelayCommand]
        private async Task DeleteLocation(object param)
        {
            if (param is not LocationItemViewModel vm)
                return;
            var dialogContent = new ConfirmDeleteDialog();
            dialogContent.OnYesCommand = new RelayCommand(() =>
            {
                _locations.Remove(vm);
                DialogHost.Close("RootDialog");
            });
            await DialogHost.Show(dialogContent, "RootDialog");
        }

        [RelayCommand]
        private async Task EditLocation(object param)
        {
            if (param is not LocationItemViewModel vm)
                return;

            var onLocationEditAction = (Location location) =>
            {
                var element = _locations.First(x => x.LocationId == vm.LocationId);
                element.Country = location.Country;
                element.City = location.City;
                Locations.Refresh();
                DialogHost.Close("RootDialog");
            };
            var dialogContent = new SubmitLocationDialog(onLocationEditAction, vm);
            await DialogHost.Show(dialogContent, "RootDialog");
        }

        public CodebookViewModel()
        {
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
            IEnumerator enumerator = Locations.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var currentItem = enumerator.Current as LocationItemViewModel;
                if(FilterLocations(currentItem))
                    currentItem.IsSelected = select;
            }
        }
        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.CODEBOOK);
    }
}
