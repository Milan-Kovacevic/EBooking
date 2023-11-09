using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
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
    public partial class UnitReservationsViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value != null && value != string.Empty)
                return;
            SearchUnitReservations();
        }

        private ObservableCollection<UnitReservationItemViewModel> _unitReservations;
        public ICollectionView UnitReservations { get; }

        public bool? IsAllItemsSelected
        {
            get
            {
                var selected = _unitReservations.Select(item => item.IsSelected).Distinct().ToList();
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

        private readonly AccommodationStore _accommodationStore;
        private readonly UnitReservationStore _unitReservationStore;
        private readonly UserStore _userStore;
        private readonly UnitReservationService _unitReservationService;
        private readonly DialogHostService _dialogHostService;

        public UnitReservationsViewModel(AccommodationStore accommodationStore, UnitReservationStore unitReservationStore, UserStore userStore, UnitReservationService unitReservationService, DialogHostService dialogHostService)
        {
            _accommodationStore = accommodationStore;
            _unitReservationStore = unitReservationStore;
            _userStore = userStore;
            _unitReservationService = unitReservationService;
            _dialogHostService = dialogHostService;

            _unitReservationStore.UnitReservationLoaded += OnUnitReservationLoaded;
            _unitReservationStore.UnitReservationAdded += OnUnitReservationAdded;
            _unitReservationStore.UnitReservationUpdated += OnUnitReservationUpdated;
            _unitReservationStore.UnitReservationDeleted += OnUnitReservationDeleted;
            searchText = string.Empty;
            isAdmin = userStore.IsAdmin;
            _unitReservations = new ObservableCollection<UnitReservationItemViewModel>();
            UnitReservations = CollectionViewSource.GetDefaultView(_unitReservations);
            UnitReservations.Filter = FilterUnitReservations;
            LoadUnitReservations();
        }

        [ObservableProperty]
        private bool isAdmin;

        public void Dispose()
        {
            _unitReservationStore.UnitReservationLoaded -= OnUnitReservationLoaded;
            _unitReservationStore.UnitReservationAdded -= OnUnitReservationAdded;
            _unitReservationStore.UnitReservationUpdated -= OnUnitReservationUpdated;
            _unitReservationStore.UnitReservationDeleted -= OnUnitReservationDeleted;
        }

        #region Unit Reservations CRUD Commands
        public async void LoadUnitReservations()
        {
            var id = _accommodationStore.SelectedAccommodation?.AccommodationId ?? 0;
            await _unitReservationStore.LoadUnitsForAccommodation(id);
        }

        private void OnUnitReservationLoaded()
        {
            _unitReservations.Clear();
            foreach (var item in _unitReservationStore.UnitReservations)
                AddNewUnitReservationItem(item);
        }

        private void OnUnitReservationAdded(UnitReservation unitReservation)
        {
            AddNewUnitReservationItem(unitReservation);
        }

        private void AddNewUnitReservationItem(UnitReservation unitReservation)
        {
            var unitReservationItem = new UnitReservationItemViewModel(unitReservation, _userStore.CurrentUser);
            _unitReservations.Add(unitReservationItem);
        }

        private void OnUnitReservationUpdated(UnitReservation unitReservation)
        {
            var unitReservationItem = _unitReservations.FirstOrDefault(f => f.UnitReservationId == unitReservation.UnitReservationId);
            Mapper.Map(unitReservation).Over(unitReservationItem);
        }

        private void OnUnitReservationDeleted(int unitReservationId)
        {
            var unitReservationItemVm = _unitReservations.FirstOrDefault(f => f.UnitReservationId == unitReservationId);
            if (unitReservationItemVm is not null)
                _unitReservations.Remove(unitReservationItemVm);
        }

        [RelayCommand]
        public void SearchUnitReservations()
        {
            try
            {
                UnitReservations.Refresh();
            }
            catch
            { }
        }

        [RelayCommand]
        public void EditUnitReservation(object param)
        {
            if (param is not UnitReservationItemViewModel vm)
                return;
        }

        [RelayCommand]
        public void DeleteUnitReservation(object param)
        {
            if (param is not UnitReservationItemViewModel vm)
                return;
        }

        [RelayCommand]
        public void DeleteSelectedUnitReservations()
        {
            _dialogHostService.OpenConfirmMultiDeleteDialog();
        }

        #endregion

        #region ViewModel Helper Functions
        private bool FilterUnitReservations(object obj)
        {
            if (obj is null || obj is not UnitReservationItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.OnName.ToLower().Contains(SearchText.ToLower());
        }

        private void SelectAll(bool select)
        {
            foreach (var item in UnitReservations)
            {
                if (item is UnitReservationItemViewModel vm)
                {
                    if (FilterUnitReservations(vm))
                        vm.IsSelected = select;
                }
            }
        }
        #endregion
    }
}
