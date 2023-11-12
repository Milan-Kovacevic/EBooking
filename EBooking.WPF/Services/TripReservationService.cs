using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class TripReservationService
    {
        private readonly TripReservationStore _tripReservationStore;

        public TripReservationService(TripReservationStore tripReservationStore)
        {
            _tripReservationStore = tripReservationStore;
        }

        public Task AddUnitReservation(TripReservation tripReservation)
        {
            return _tripReservationStore.Insert(tripReservation);
        }

        public Task UpdateUnitReservation(TripReservation tripReservation)
        {
            if (_tripReservationStore.TripReservations.Any(x => x.TripReservationId == tripReservation.TripReservationId))
                return _tripReservationStore.Update(tripReservation);
            return Task.CompletedTask;
        }

        public Task DeleteUnitReservation(int tripReservationId)
        {
            if (_tripReservationStore.TripReservations.Any(x => x.TripReservationId == tripReservationId))
                return _tripReservationStore.Delete(tripReservationId);
            return Task.CompletedTask;
        }

        public TripReservation? GetSelectedUnitReservation()
        {
            return _tripReservationStore.SelectedTripReservation;
        }

        public void SetSelectedUnitReservation(int tripReservationId)
        {
            var result = _tripReservationStore.TripReservations.FirstOrDefault(x => x.TripReservationId == tripReservationId);
            _tripReservationStore.SelectedTripReservation = result;
        }
    }
}
