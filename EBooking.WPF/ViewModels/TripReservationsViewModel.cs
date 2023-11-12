using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EBooking.Domain.DTOs;
using EBooking.WPF.ItemViewModels;
using EBooking.WPF.Messages;
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
    public partial class TripReservationsViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value != null && value != string.Empty)
                return;
            SearchTripReservations();
        }

        private ObservableCollection<TripReservationItemViewModel> _tripReservations;
        public ICollectionView TripReservations { get; }

        public bool? IsAllItemsSelected
        {
            get
            {
                var selected = _tripReservations.Select(item => item.IsSelected).Distinct().ToList();
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

        private readonly UserStore _userStore;
        private readonly DialogHostService _dialogHostService;
        private readonly MessageQueueService _messageQueueService;

        public TripReservationsViewModel(UserStore userStore, DialogHostService dialogHostService, MessageQueueService messageQueueService)
        {
            _userStore = userStore;
            _dialogHostService = dialogHostService;
            _messageQueueService = messageQueueService;

            searchText = string.Empty;
            isAdmin = userStore.IsAdmin;
            isEmployee = userStore.IsEmployee;
            _tripReservations = new ObservableCollection<TripReservationItemViewModel>()
            {
                new TripReservationItemViewModel(){ IsOwner = true, TripType="One-Way", OnName = "Marko Markovic", NumberOfSeats = 1, ReservedBy = "Test1", TotalPrice = 100.99m, TripSummary = "Belgrade - Banja Luka"},
                new TripReservationItemViewModel(){ IsOwner = true, TripType="Two-Way", OnName = "Stefan Markovic", NumberOfSeats = 2, ReservedBy = "Test2", TotalPrice = 320.99m, TripSummary = "Belgrade - Banja Luka"}
            };
            TripReservations = CollectionViewSource.GetDefaultView(_tripReservations);
            TripReservations.Filter = FilterTripReservations;
            LoadTripReservations();
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        [ObservableProperty]
        private bool isAdmin;
        [ObservableProperty]
        private bool isEmployee;

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        #region Unit Reservations CRUD Commands
        public async void LoadTripReservations()
        {

        }

        [RelayCommand]
        public void SearchTripReservations()
        {
            try
            {
                TripReservations.Refresh();
            }
            catch
            { }
        }

        [RelayCommand]
        public void AddTripReservation()
        {
            _dialogHostService.OpenAddTripReservation();
        }

        [RelayCommand]
        public void EditTripReservation(object param)
        {
            if (param is not TripReservationItemViewModel vm)
                return;
        }

        [RelayCommand]
        public void DeleteTripReservation(object param)
        {
            if (param is not TripReservationItemViewModel vm)
                return;
        }

        [RelayCommand]
        public void DeleteSelectedTripReservations()
        {
            _dialogHostService.OpenConfirmMultiDeleteDialog();
        }

        public async void Receive(DeleteSelectedDialogResultMessage message)
        {
            if (message.DialogResult == false)
                return;
            List<Task> tasks = new List<Task>();
            for (int i = _tripReservations.Count - 1; i >= 0; i--)
            {
                if (_tripReservations[i].IsSelected)
                {
                    //tasks.Add(_unitReservationService.DeleteUnitReservation(_tripReservations[i].TripReservationId));
                }
            }
            await Task.WhenAll(tasks);
            IsAllItemsSelected = false;
        }
        #endregion

        #region ViewModel Helper Functions
        private bool FilterTripReservations(object obj)
        {
            if (obj is null || obj is not TripReservationItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.OnName.ToLower().Contains(SearchText.ToLower());
        }

        private void SelectAll(bool select)
        {
            foreach (var item in TripReservations)
            {
                if (item is TripReservationItemViewModel vm)
                {
                    if (FilterTripReservations(vm))
                        vm.IsSelected = select;
                }
            }
        }

        #endregion
    }
}
