using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class UnitReservationItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private bool isOwner;
        [ObservableProperty]
        private int unitReservationId;
        [ObservableProperty]
        private string onName;
        [ObservableProperty]
        private string reservationFromText;
        [ObservableProperty]
        private string reservationToText;
        [ObservableProperty]
        private string numberOfPeople;
        [ObservableProperty]
        private decimal totalPrice;
        [ObservableProperty]
        private string reservedBy;
        [ObservableProperty]
        private string unitName;

        private int _numberOfAdults;
        public int NumberOfAdults
        {
            get => _numberOfAdults;
            set
            {
                _numberOfAdults = value;
                SetNumberOfPeople();
            }
        }

        private int _numberOfChildren;
        public int NumberOfChildren
        {
            get => _numberOfChildren;
            set
            {
                _numberOfChildren = value;
                SetNumberOfPeople();
            }
        }

        private DateTime? _reservationFrom;
        public DateTime? ReservationFrom
        {
            get => _reservationFrom;
            set
            {
                _reservationFrom = value;
                ReservationFromText = value?.Date.ToShortDateString() ?? string.Empty;
            }
        }

        private DateTime? _reservationTo;
        public DateTime? ReservationTo
        {
            get => _reservationTo;
            set
            {
                _reservationTo = value;
                ReservationToText = value?.Date.ToShortDateString() ?? string.Empty;
            }
        }

        public UnitReservationItemViewModel(UnitReservation unitReservation, User? currentUser)
        {
            isSelected = false;
            isOwner = false;
            unitReservationId = 0;
            onName = string.Empty;
            reservationFromText = string.Empty;
            reservationToText = string.Empty;
            numberOfPeople = string.Empty;
            _numberOfAdults = 0;
            _numberOfChildren = 0;
            ReservationFrom = null;
            ReservationTo = null;
            reservedBy = $"{unitReservation.Employee?.FirstName} {unitReservation.Employee?.LastName}";
            unitName = $"{unitReservation.Unit?.Name}";
            isOwner = unitReservation.EmployeeId == currentUser?.UserId;
            Mapper.Map(unitReservation).Over(this);
        }

        private void SetNumberOfPeople()
        {
            NumberOfPeople = $"{NumberOfAdults} Adults, {NumberOfChildren} Children";
        }
    }
}
