using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.WPF.Services;
using EBooking.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class ReservationsViewModel : ObservableObject, IViewModelBase
    {
        public AccommodationReservationsViewModel AccommodationReservationsViewModel { get; set; }
        public TripReservationsViewModel TripReservationsViewModel { get; set; }

        [ObservableProperty]
        private int selectedTabMenuIndex;
        partial void OnSelectedTabMenuIndexChanged(int value)
        {
            if (IsUnitReservationsView)
                AccommodationReservationsViewModel.LoadUnitReservations();
        }
        private bool IsUnitReservationsView { get => SelectedTabMenuIndex == 1; }

        public ReservationsViewModel(AccommodationReservationsViewModel accommodationReservationsViewModel, TripReservationsViewModel tripReservationsViewModel)
        {
            AccommodationReservationsViewModel = accommodationReservationsViewModel;
            TripReservationsViewModel = tripReservationsViewModel;
        }

        public void Dispose()
        {
            AccommodationReservationsViewModel?.Dispose();
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.RESERVATIONS);
    }
}
