using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class UnitReservationService
    {
        private readonly UnitReservationStore _unitReservationStore;

        public UnitReservationService(UnitReservationStore unitReservationStore)
        {
            _unitReservationStore = unitReservationStore;
        }

        public Task AddUnitReservation(UnitReservation unitReservation)
        {
            return _unitReservationStore.Insert(unitReservation);
        }

        public Task UpdateUnitReservation(UnitReservation unitReservation)
        {
            if (_unitReservationStore.UnitReservations.Any(x => x.UnitReservationId == unitReservation.UnitReservationId))
                return _unitReservationStore.Update(unitReservation);
            return Task.CompletedTask;
        }

        public Task DeleteAccommodationUnit(int unitReservationId)
        {
            if (_unitReservationStore.UnitReservations.Any(x => x.UnitReservationId == unitReservationId))
                return _unitReservationStore.Delete(unitReservationId);
            return Task.CompletedTask;
        }

    }
}
