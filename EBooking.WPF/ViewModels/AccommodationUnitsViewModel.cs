using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.ItemViewModels;
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
    public partial class AccommodationUnitsViewModel : ObservableObject, IViewModelBase
    {
        private ObservableCollection<AccommodationUnitItemViewModel> _accommodationUnits;
        public ICollectionView AccommodationUnits { get; }

        private readonly AccommodationUnitStore _accommodationUnitStore;
        private readonly DialogHostService _dialogHostService;
        private readonly AccommodationUnitService _accommodationUnitService;
        private readonly MessageQueueService _messageQueueService;

        public AccommodationUnitsViewModel(AccommodationStore accommodationStore, AccommodationUnitStore accommodationUnitStore, UserStore userStore, DialogHostService dialogHostService, AccommodationUnitService accommodationUnitService, MessageQueueService messageQueueService)
        {
            _accommodationUnitStore = accommodationUnitStore;
            _dialogHostService = dialogHostService;
            _accommodationUnitService = accommodationUnitService;
            _messageQueueService = messageQueueService;

            _accommodationUnitStore.AccommodationUnitLoaded += OnAccommodationUnitsLoaded;
            _accommodationUnitStore.AccommodationUnitAdded += OnAccommodationUnitAdded;
            _accommodationUnitStore.AccommodationUnitUpdated += OnAccommodationUnitsUpdated;
            _accommodationUnitStore.AccommodationUnitDeleted += OnAccommodationUnitsDeleted;

            _accommodationUnits = new ObservableCollection<AccommodationUnitItemViewModel>();
            AccommodationUnits = CollectionViewSource.GetDefaultView(_accommodationUnits);
            isAdminOwner = accommodationStore.SelectedAccommodation?.UserId == userStore.CurrentUser?.UserId;
            isEmployee = userStore.IsEmployee;
            _accommodationUnitStore.CurrentAccommodation = accommodationStore.SelectedAccommodation;
        }

        public void Dispose()
        {
            _accommodationUnitStore.AccommodationUnitLoaded -= OnAccommodationUnitsLoaded;
            _accommodationUnitStore.AccommodationUnitAdded -= OnAccommodationUnitAdded;
            _accommodationUnitStore.AccommodationUnitUpdated -= OnAccommodationUnitsUpdated;
            _accommodationUnitStore.AccommodationUnitDeleted -= OnAccommodationUnitsDeleted;
        }

        [ObservableProperty]
        private bool isAdminOwner;
        [ObservableProperty]
        private bool isEmployee;

        #region Accommodation Units CRUD Commands
        private void OnAccommodationUnitsLoaded()
        {
            _accommodationUnits.Clear();
            foreach (var item in _accommodationUnitStore.AccommodationUnits)
                AddNewAccommodationUnitItem(item);
        }

        private void OnAccommodationUnitAdded(AccommodationUnit accommodationUnit)
        {
            AddNewAccommodationUnitItem(accommodationUnit);
            var message = LanguageTranslator.Translate(LanguageTranslator.MessageType.ACCOMMODATION_UNIT_ADDED);
            _messageQueueService.Enqueue($"{message} ' {accommodationUnit.Name} '");
        }

        private void AddNewAccommodationUnitItem(AccommodationUnit accommodationUnit)
        {
            var accommodationUnitItem = new AccommodationUnitItemViewModel(_accommodationUnitService, _dialogHostService);
            Mapper.Map(accommodationUnit).Over(accommodationUnitItem);
            _accommodationUnits.Add(accommodationUnitItem);
        }

        private void OnAccommodationUnitsUpdated(AccommodationUnit accommodationUnit)
        {
            var accommodationUnitItemVm = _accommodationUnits.FirstOrDefault(f => f.UnitId == accommodationUnit.UnitId);
            Mapper.Map(accommodationUnit).Over(accommodationUnitItemVm);
            var message = LanguageTranslator.Translate(LanguageTranslator.MessageType.ACCOMMODATION_UNIT_UPDATED);
            _messageQueueService.Enqueue($"{message} ' {accommodationUnit.Name} '");
        }

        private void OnAccommodationUnitsDeleted(int unitId)
        {
            var accommodationUnitItemVm = _accommodationUnits.FirstOrDefault(f => f.UnitId == unitId);
            if (accommodationUnitItemVm is not null)
            {
                _accommodationUnits.Remove(accommodationUnitItemVm);
                var message = LanguageTranslator.Translate(LanguageTranslator.MessageType.ACCOMMODATION_UNIT_DELETED);
                _messageQueueService.Enqueue($"{message} ' {accommodationUnitItemVm.Name} '");
            }              
        }

        [RelayCommand]
        public void AddAccommodationUnit()
        {
            _dialogHostService.OpenAccommodationUnitAddDialog();
        }

        #endregion
    }
}
