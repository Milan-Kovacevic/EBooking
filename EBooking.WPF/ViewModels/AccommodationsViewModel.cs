using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
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
        [ObservableProperty]
        private bool isAdmin;

        private readonly AccommodationStore _accommodationStore;
        private readonly AccommodationService _accommodationService;
        private readonly DialogHostService _dialogHostService;
        private readonly UserStore _userStore;

        public AccommodationsViewModel(AccommodationStore accommodationStore, AccommodationService accommodationService, DialogHostService dialogHostService, UserStore userStore)
        {
            _accommodationStore = accommodationStore;
            _accommodationService = accommodationService;
            _dialogHostService = dialogHostService;
            _userStore = userStore;
            _accommodationStore.AccommodationAdded += OnAccommodationAdded;
            _accommodationStore.AccommodationUpdated += OnAccommodationUpdated;
            _accommodationStore.AccommodationDeleted += OnAccommodationDeleted;

            searchText = string.Empty;
            isFilterSelected = false;
            isAdmin = userStore.IsAdmin;
            _accommodations = new ObservableCollection<AccommodationItemViewModel>(accommodationStore.Accommodations.Select(x => Mapper.Map(x).ToANew<AccommodationItemViewModel>()));
            Accommodations = CollectionViewSource.GetDefaultView(_accommodations);
            Accommodations.Filter = FilterAccommodations;
        }

        public void Dispose()
        {
            _accommodationStore.AccommodationAdded -= OnAccommodationAdded;
            _accommodationStore.AccommodationUpdated -= OnAccommodationUpdated;
            _accommodationStore.AccommodationDeleted -= OnAccommodationDeleted;
        }

        #region Accommodations CRUD Commands
        private void OnAccommodationAdded(Accommodation accommodation)
        {
            _accommodations.Add(Mapper.Map(accommodation).ToANew<AccommodationItemViewModel>());
        }

        private void OnAccommodationUpdated(Accommodation accommodation)
        {
            throw new NotImplementedException();
        }
        private void OnAccommodationDeleted(int obj)
        {
            throw new NotImplementedException();
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
            await Task.Delay(200);
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
    }
}
