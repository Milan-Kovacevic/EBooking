using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class AccommodationUnitItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isOwner;
        [ObservableProperty]
        private int unitId;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string availabilityPeriod;
        [ObservableProperty]
        private int numberOfBeds;
        [ObservableProperty]
        private decimal pricePerNight;
        public ObservableCollection<UnitFeature> Features { get; set; }


        private DateTime _availableFrom;
        public DateTime AvailableFrom
        {
            get => _availableFrom;
            set
            {
                _availableFrom = value;
                SetAvailabilityPeriod();
            }
        }

        private DateTime _availableTo;
        public DateTime AvailableTo
        {
            get => _availableTo;
            set
            {
                _availableTo = value;
                SetAvailabilityPeriod();
            }
        }

        private readonly AccommodationUnitService _accommodationUnitService;
        private readonly DialogHostService _dialogHostService;

        public AccommodationUnitItemViewModel(AccommodationUnitService accommodationUnitService, DialogHostService dialogHostService)
        {
            _accommodationUnitService = accommodationUnitService;
            _dialogHostService = dialogHostService;
            unitId = 0;
            name = string.Empty;
            availabilityPeriod = string.Empty;
            _availableFrom = DateTime.Now;
            _availableTo = DateTime.Now;
            numberOfBeds = 0;
            pricePerNight = 0.0m;
            Features = new ObservableCollection<UnitFeature>();
        }

        private void SetAvailabilityPeriod()
        {
            AvailabilityPeriod = $"{AvailableFrom:dd.MM.yyyy} - {AvailableTo:dd.MM.yyyy}";
        }


        [RelayCommand]
        public void EditAccommodationUnit(object param)
        {
            if (param is not AccommodationUnitItemViewModel vm)
                return;
            _accommodationUnitService.SetSelectedAccommodationUnit(vm.UnitId);
            _dialogHostService.OpenAccommodationUnitEditDialog();
        }

        [RelayCommand]
        public void DeleteAccommodationUnit(object param)
        {
            if (param is not AccommodationUnitItemViewModel vm)
                return;
            _accommodationUnitService.SetSelectedAccommodationUnit(vm.UnitId);
            _dialogHostService.OpenAccommodationUnitDeleteDialog();
        }

        [RelayCommand]
        public void MakeUnitReservation(object param)
        {
            if (param is not AccommodationUnitItemViewModel vm)
                return;
            _accommodationUnitService.SetSelectedAccommodationUnit(vm.UnitId);
            _dialogHostService.OpenUnitReservationAddDialog();
        }
    }
}
