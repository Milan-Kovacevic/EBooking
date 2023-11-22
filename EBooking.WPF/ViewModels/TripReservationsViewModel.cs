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
    public partial class TripReservationsViewModel : ObservableObject, IViewModelBase, IRecipient<DeleteSelectedDialogResultMessage>
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

        private readonly TripReservationStore _tripReservationStore;
        private readonly TripReservationService _tripReservationService;
        private readonly UserStore _userStore;
        private readonly DialogHostService _dialogHostService;
        private readonly MessageQueueService _messageQueueService;

        public TripReservationsViewModel(TripReservationStore tripReservationStore, TripReservationService tripReservationService, UserStore userStore, DialogHostService dialogHostService, MessageQueueService messageQueueService)
        {
            _tripReservationStore = tripReservationStore;
            _tripReservationService = tripReservationService;
            _userStore = userStore;
            _dialogHostService = dialogHostService;
            _messageQueueService = messageQueueService;

            _tripReservationStore.TripReservationLoaded += OnTripReservationLoaded;
            _tripReservationStore.TripReservationAdded += OnTripReservationAdded;
            _tripReservationStore.TripReservationUpdated += OnTripReservationUpdated;
            _tripReservationStore.TripReservationDeleted += OnTripReservationDeleted;

            searchText = string.Empty;
            isAdmin = userStore.IsAdmin;
            isEmployee = userStore.IsEmployee;
            _tripReservations = new ObservableCollection<TripReservationItemViewModel>();
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
            _tripReservationStore.TripReservationLoaded -= OnTripReservationLoaded;
            _tripReservationStore.TripReservationAdded -= OnTripReservationAdded;
            _tripReservationStore.TripReservationUpdated -= OnTripReservationUpdated;
            _tripReservationStore.TripReservationDeleted -= OnTripReservationDeleted;
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        #region Unit Reservations CRUD Commands
        public async void LoadTripReservations()
        {
            await _tripReservationStore.Load();
        }

        private void OnTripReservationLoaded()
        {
            _tripReservations.Clear();
            foreach (var item in _tripReservationStore.TripReservations)
                AddNewTripReservationItem(item);
        }

        private void OnTripReservationAdded(TripReservation tripReservation)
        {
            AddNewTripReservationItem(tripReservation);
            var message = LanguageTranslator.Translate(LanguageTranslator.MessageType.TRIP_RESERVATION_ADDED);
            _messageQueueService.Enqueue($"{message} ' {tripReservation.OnName} '");
        }

        private void AddNewTripReservationItem(TripReservation tripReservation)
        {
            var tripReservationItem = new TripReservationItemViewModel(tripReservation, _userStore.CurrentUser);
            _tripReservations.Add(tripReservationItem);
        }

        private void OnTripReservationUpdated(TripReservation tripReservation)
        {
            var tripReservationItem = _tripReservations.FirstOrDefault(f => f.TripReservationId == tripReservation.TripReservationId);
            Mapper.Map(tripReservation).Over(tripReservationItem);
            tripReservationItem?.SetTripSummary();
            var message = LanguageTranslator.Translate(LanguageTranslator.MessageType.TRIP_RESERVATION_UPDATED);
            _messageQueueService.Enqueue($"{message} ' {tripReservation.OnName} '");
        }

        private void OnTripReservationDeleted(int tripReservationId)
        {
            var tripReservationItemVm = _tripReservations.FirstOrDefault(f => f.TripReservationId == tripReservationId);
            if (tripReservationItemVm is not null)
            {
                _tripReservations.Remove(tripReservationItemVm);
                var message = LanguageTranslator.Translate(LanguageTranslator.MessageType.TRIP_RESERVATION_DELETED);
                _messageQueueService.Enqueue($"{message} ' {tripReservationItemVm.OnName} '");
            }
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
            _dialogHostService.OpenTripReservationAddDialog();
        }

        [RelayCommand]
        public void EditTripReservation(object param)
        {
            if (param is not TripReservationItemViewModel vm)
                return;
            _tripReservationService.SetSelectedTripReservation(vm.TripReservationId);
            _dialogHostService.OpenTripReservationEditDialog();
        }

        [RelayCommand]
        public void DeleteTripReservation(object param)
        {
            if (param is not TripReservationItemViewModel vm)
                return;
            _tripReservationService.SetSelectedTripReservation(vm.TripReservationId);
            _dialogHostService.OpenTripReservationDeleteDialog();
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
                    tasks.Add(_tripReservationService.DeleteTripReservation(_tripReservations[i].TripReservationId));
                }
            }
            await Task.WhenAll(tasks);
            IsAllItemsSelected = false;
        }

        public void ShowReservationDetailsFor(TripReservationItemViewModel selectedItem)
        {
            _tripReservationService.SetSelectedTripReservation(selectedItem.TripReservationId);
            _dialogHostService.OpenTripReservationDetailsDialog();
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
