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

        private readonly DialogHostService _dialogHostService;

        public UnitReservationsViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
            searchText = string.Empty;
            _unitReservations = new ObservableCollection<UnitReservationItemViewModel>()
            {
                new UnitReservationItemViewModel(){ OnName = "Marko Markovic", ReservationPeriod = "10.10.2023 - 12.10.2023", NumberOfPeople = "2 Adults, 1 Children", TotalPrice = 120m, IsOwner = true },
                new UnitReservationItemViewModel(){ OnName = "Janko Jankovic", ReservationPeriod = "24.10.2023 - 02.11.2023", NumberOfPeople = "4 Adults, 2 Children", TotalPrice = 120m, IsOwner = false }
            };
            UnitReservations = CollectionViewSource.GetDefaultView(_unitReservations);
            UnitReservations.Filter = FilterUnitReservations;
        }

        public void Dispose()
        {

        }

        #region Unit Reservations CRUD Commands

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
