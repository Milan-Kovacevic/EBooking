﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class AccommodationDetailsViewModel : ObservableObject, IViewModelBase
    {
        public AccommodationUnitsViewModel AccommodationUnitsViewModel { get; set; }
        public UnitReservationsViewModel UnitReservationsViewModel { get; set; }
        [ObservableProperty]
        private string accommodationName;
        [ObservableProperty]
        private string accommodationAddress;
        [ObservableProperty]
        private string accommodationLocation;
        [ObservableProperty]
        private int selectedTabMenuIndex;
        partial void OnSelectedTabMenuIndexChanged(int value)
        {
            if (IsUnitReservationsView)
                UnitReservationsViewModel.LoadUnitReservations();
        }
        private bool IsUnitReservationsView { get => SelectedTabMenuIndex == 1; }
        private readonly NavigationService _navigateToAccommodationsViewModel;

        public AccommodationDetailsViewModel(AccommodationStore accommodationStore, NavigationService navigateToAccommodationsViewModel, AccommodationUnitsViewModel accommodationUnitsViewModel, UnitReservationsViewModel unitReservationsViewModel)
        {
            _navigateToAccommodationsViewModel = navigateToAccommodationsViewModel;
            accommodationName = accommodationStore.SelectedAccommodation?.Name ?? string.Empty;
            accommodationAddress = accommodationStore.SelectedAccommodation?.Address ?? string.Empty;
            var location = accommodationStore.SelectedAccommodation?.Location;
            accommodationLocation = $"{location?.Country}, {location?.City}";
            selectedTabMenuIndex = 0;
            AccommodationUnitsViewModel = accommodationUnitsViewModel;
            UnitReservationsViewModel = unitReservationsViewModel;
        }

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToAccommodationsViewModel.Navigate();
        }

        public void Dispose()
        {
            AccommodationUnitsViewModel?.Dispose();
            UnitReservationsViewModel?.Dispose();
        }
    }
}
