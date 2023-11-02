using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Services;
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
    public partial class AccommodationsViewModel : ObservableObject, IViewModelBase
    {
        private readonly ObservableCollection<AccommodationItemViewModel> _accommodations;
        public ICollectionView Accommodations { get; }

        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value != null && value != string.Empty)
                return;
            SearchAccommodations();
        }
        [ObservableProperty]
        private bool isFilterSelected;


        [RelayCommand]
        public void SearchAccommodations()
        {
            try
            {
                Accommodations.Refresh();
            }
            catch
            { }
        }

        [RelayCommand]
        public void AddAccommodation()
        {

        }

        [RelayCommand]
        public async Task FilterAccommodations()
        {
            await _dialogHostService.ShowFilterAccommodationsDialog();
        }

        private readonly DialogHostService _dialogHostService;

        public AccommodationsViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
            searchText = string.Empty;
            isFilterSelected = false;
            _accommodations = new ObservableCollection<AccommodationItemViewModel>()
            {
                new AccommodationItemViewModel() {Name="Apartment1", Type = "Apartment", Address = "Address template 1", Location = "Serbia, Belgrade", NumOfUnits = 2},
                new AccommodationItemViewModel() {Name="Apartment2, Apartment3", Type = "Apartment", Address = "Address template 19", Location = "Serbia, Belgrade", NumOfUnits = 0},
                new AccommodationItemViewModel() {Name="Long name hotel1", Type = "Hotel", Address = "Address test 2", Location = "Serbia, Novi Sad", NumOfUnits = 3},
                new AccommodationItemViewModel() {Name="Apartment4", Type = "Apartment", Address = "Address template 3, address template 3...", Location = "Serbia, Novi Sad", NumOfUnits = 1},
                new AccommodationItemViewModel() {Name="Hotel name2", Type = "Hotel", Address = "Address test 3", Location = "Bosnia and Herzegovina, Banja Luka", NumOfUnits = 3},
            };
            Accommodations = CollectionViewSource.GetDefaultView(_accommodations);
            Accommodations.Filter = FilterAccommodations;
        }

        private bool FilterAccommodations(object obj)
        {
            if (obj is null || obj is not AccommodationItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.Name.ToLower().Contains(SearchText.ToLower()) || vm.Address.ToLower().Contains(SearchText.ToLower());
        }
    }
}
