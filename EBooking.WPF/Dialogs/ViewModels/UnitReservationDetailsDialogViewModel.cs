using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.WPF.Services;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class UnitReservationDetailsDialogViewModel : ObservableObject, IViewModelBase
    {
        public string OnName { get; }
        public string ReservationFrom { get; }
        public string ReservationTo { get; }
        public string NumberOfPeople { get; }
        public string ReservedBy { get; }
        public string TotalPrice { get; }
        public string UnitName { get; }
        public string UnitAvailableFrom { get; }
        public string UnitAvailableTo { get; }
        public string UnitNumberOfBeds { get; }
        public string UnitPricePerNight { get; }

        private readonly static string DETAILS_PLACEHOLDER_VALUE = "-";

        public UnitReservationDetailsDialogViewModel(UnitReservationService unitReservationService)
        {
            var unitReservation = unitReservationService.GetSelectedUnitReservation();
            OnName = unitReservation?.OnName ?? DETAILS_PLACEHOLDER_VALUE;
            ReservationFrom = unitReservation?.ReservationFrom.ToLongDateString() ?? DETAILS_PLACEHOLDER_VALUE;
            ReservationTo = unitReservation?.ReservationTo.ToLongDateString() ?? DETAILS_PLACEHOLDER_VALUE;
            NumberOfPeople = GetNumberOfPeople(unitReservation?.NumberOfAdults ?? 0, unitReservation?.NumberOfChildren ?? 0);
            ReservedBy = $"{unitReservation?.Employee?.FirstName} {unitReservation?.Employee?.LastName}";
            TotalPrice = unitReservation?.TotalPrice.ToString() ?? "0.0";
            UnitName = unitReservation?.Unit?.Name ?? DETAILS_PLACEHOLDER_VALUE;
            UnitAvailableFrom = unitReservation?.Unit?.AvailableFrom.ToLongDateString() ?? DETAILS_PLACEHOLDER_VALUE;
            UnitAvailableTo = unitReservation?.Unit?.AvailableTo.ToLongDateString() ?? DETAILS_PLACEHOLDER_VALUE;
            UnitNumberOfBeds = unitReservation?.Unit?.NumberOfBeds.ToString() ?? DETAILS_PLACEHOLDER_VALUE;
            UnitPricePerNight = unitReservation?.Unit?.PricePerNight.ToString() ?? "0.0";
        }

        private static string GetNumberOfPeople(int numOfAdults, int numOfChildren)
        {
            return $"{numOfAdults} Adults, {numOfChildren} Children";
        }

    }
}
