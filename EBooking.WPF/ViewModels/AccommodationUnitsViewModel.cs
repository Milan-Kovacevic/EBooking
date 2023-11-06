﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class AccommodationUnitsViewModel : ObservableObject, IViewModelBase
    {
        private ObservableCollection<AccommodationUnitItemViewModel> _accommodationUnits;
        public ICollectionView AccommodationUnits { get; }

        private readonly AccommodationStore _accommodationStore;
        private readonly NavigationService _navigateToAccommodationsViewModel;

        public AccommodationUnitsViewModel(AccommodationStore accommodationStore, NavigationService navigateToAccommodationsViewModel, UserStore userStore)
        {
            _accommodationStore = accommodationStore;
            _navigateToAccommodationsViewModel = navigateToAccommodationsViewModel;
            _accommodationUnits = new ObservableCollection<AccommodationUnitItemViewModel>() {
                new AccommodationUnitItemViewModel(){
                    Name = "Apartment building 1",
                    NumberOfBeds = 1,
                    PricePerNight = 250.99m,
                    AvailableFrom = new DateTime(2023, 2, 1),
                    AvailableTo = new DateTime(2023, 7, 1),
                    IsOwner = true,
                Features = new ObservableCollection<UnitFeature>(){
                    new UnitFeature(){ Name = "Free WiFi" },
                    new UnitFeature(){ Name = "Spacious Garage" },
                } },
                new AccommodationUnitItemViewModel(){
                    Name = "Apartment building 2",
                    NumberOfBeds = 4,
                    PricePerNight = 810.49m,
                    AvailableFrom = new DateTime(2023, 2, 1),
                    AvailableTo = new DateTime(2023, 7, 1),
                    IsOwner = true },
            };
            AccommodationUnits = CollectionViewSource.GetDefaultView(_accommodationUnits);

            isAdminOwner = userStore.IsAdmin;
            accommodationName = _accommodationStore.SelectedAccommodation?.Name ?? string.Empty;
            accommodationAddress = _accommodationStore.SelectedAccommodation?.Address ?? string.Empty;
            var location = _accommodationStore.SelectedAccommodation?.Location;
            accommodationLocation = $"{location?.Country}, {location?.City}";

            isAdminOwner = _accommodationStore.SelectedAccommodation?.UserId == userStore.CurrentUser?.UserId;
        }

        public void Dispose()
        {

        }

        [ObservableProperty]
        private bool isAdminOwner;
        [ObservableProperty]
        private string accommodationName;
        [ObservableProperty]
        private string accommodationAddress;
        [ObservableProperty]
        private string accommodationLocation;

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToAccommodationsViewModel.Navigate();
        }

        [RelayCommand]
        public void AddAccommodationUnit()
        {

        }

        [RelayCommand]
        public void EditAccommodationUnit(object param)
        {

        }

        [RelayCommand]
        public void DeleteAccommodationUnit(object param)
        {

        }
    }
}
