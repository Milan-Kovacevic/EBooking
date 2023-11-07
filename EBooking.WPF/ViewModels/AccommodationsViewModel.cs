using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.ViewModels
{
    public partial class AccommodationsViewModel : ObservableObject, IViewModelBase
    {
        private ObservableCollection<AccommodationItemViewModel> _accommodations;
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
        [ObservableProperty]
        private bool isAdmin;

        private readonly AccommodationStore _accommodationStore;
        private readonly UserStore _userStore;
        private readonly AccommodationService _accommodationService;
        private readonly DialogHostService _dialogHostService;
        private readonly NavigationService _navigateToAccommodationUnitsViewModel;

        public AccommodationsViewModel(AccommodationStore accommodationStore, AccommodationService accommodationService, DialogHostService dialogHostService, UserStore userStore, NavigationService navigateToAccommodationUnitsViewModel)
        {
            _accommodationStore = accommodationStore;
            _userStore = userStore;
            _accommodationService = accommodationService;
            _dialogHostService = dialogHostService;
            _navigateToAccommodationUnitsViewModel = navigateToAccommodationUnitsViewModel;
            _accommodationStore.AccommodationLoaded += OnAccommodationLoaded;
            _accommodationStore.AccommodationAdded += OnAccommodationAdded;
            _accommodationStore.AccommodationUpdated += OnAccommodationUpdated;
            _accommodationStore.AccommodationDeleted += OnAccommodationDeleted;

            searchText = string.Empty;
            isFilterSelected = false;
            isAdmin = userStore.IsAdmin;
            _accommodations = new ObservableCollection<AccommodationItemViewModel>();
            Accommodations = CollectionViewSource.GetDefaultView(_accommodations);
            Accommodations.Filter = FilterAccommodations;
            LoadAccommodations();
        }

        public async void LoadAccommodations()
        {
            await _accommodationStore.Load();
        }

        public void Dispose()
        {
            _accommodationStore.AccommodationLoaded -= OnAccommodationLoaded;
            _accommodationStore.AccommodationAdded -= OnAccommodationAdded;
            _accommodationStore.AccommodationUpdated -= OnAccommodationUpdated;
            _accommodationStore.AccommodationDeleted -= OnAccommodationDeleted;
        }

        #region Accommodations CRUD Commands
        private void OnAccommodationLoaded()
        {
            _accommodations.Clear();
            foreach (var item in _accommodationStore.Accommodations)
                AddNewAccommodationItem(item);
        }

        private void OnAccommodationAdded(Accommodation accommodation)
        {
            AddNewAccommodationItem(accommodation);
        }

        private void AddNewAccommodationItem(Accommodation accommodation)
        {
            var accommodationItem = new AccommodationItemViewModel(_accommodationService, _dialogHostService);
            Mapper.Map(accommodation).Over(accommodationItem);
            accommodationItem.IsOwner = _userStore.CurrentUser?.UserId == accommodation.UserId;
            accommodationItem.NumOfUnits = accommodation.AccommodationUnits.Count;
            _accommodations.Add(accommodationItem);
        }

        private void OnAccommodationUpdated(Accommodation accommodation)
        {
            var accommodationItemVm = _accommodations.FirstOrDefault(f => f.AccommodationId == accommodation.AccommodationId);
            Mapper.Map(accommodation).Over(accommodationItemVm);
        }

        private void OnAccommodationDeleted(int id)
        {
            var accommodationItemVm = _accommodations.FirstOrDefault(f => f.AccommodationId == id);
            if (accommodationItemVm is not null)
                _accommodations.Remove(accommodationItemVm);
        }

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
            _dialogHostService.OpenAccommodationAddDialog();
        }

        [RelayCommand]
        public async Task FilterAccommodations()
        {
            // TODO
            await Task.Delay(200);
        }

        public void ShowAccommodationDetails(object param)
        {
            if (param is not AccommodationItemViewModel vm)
                return;

            _accommodationService.SetSelectedAccommodation(vm.AccommodationId);
            _navigateToAccommodationUnitsViewModel.Navigate();
        }
        #endregion

        private bool FilterAccommodations(object obj)
        {
            if (obj is null || obj is not AccommodationItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.Name.ToLower().Contains(SearchText.ToLower()) || vm.Address.ToLower().Contains(SearchText.ToLower());
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.ACCOMMODATIONS);
    }
}
