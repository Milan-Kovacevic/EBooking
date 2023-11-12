using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class TripReservationStore
    {
        private readonly ITripReservationDAO _tripReservationDao;
        private readonly List<TripReservation> _tripReservations;

        public IEnumerable<TripReservation> TripReservations { get => _tripReservations; }
        public TripReservation? SelectedTripReservation { get; set; }
        public event Action? TripReservationLoaded;
        public event Action<TripReservation>? TripReservationAdded;
        public event Action<TripReservation>? TripReservationUpdated;
        public event Action<int>? TripReservationDeleted;

        public TripReservationStore(ITripReservationDAO tripReservationDao)
        {
            _tripReservationDao = tripReservationDao;
            _tripReservations = new List<TripReservation>();
        }

        public async Task Load()
        {
            var loadedTripReservations = await _tripReservationDao.GetAll();
            _tripReservations.Clear();
            _tripReservations.AddRange(loadedTripReservations);

            TripReservationLoaded?.Invoke();
        }

        public async Task Insert(TripReservation tripReservation)
        {
            var result = await _tripReservationDao.Insert(tripReservation);
            _tripReservations.Add(result);

            TripReservationAdded?.Invoke(result);
        }

        public async Task Update(TripReservation tripReservation)
        {
            await _tripReservationDao.Update(tripReservation);
            var result = await _tripReservationDao.GetById(tripReservation.TripReservationId);
            int tripReservationIndex = _tripReservations.FindIndex(f => f.TripReservationId == result.TripReservationId);
            if (tripReservationIndex == -1)
                _tripReservations.Add(result);
            else
                _tripReservations[tripReservationIndex] = result;

            TripReservationUpdated?.Invoke(result);
        }

        public async Task Delete(int id)
        {
            await _tripReservationDao.Delete(id);
            _tripReservations.RemoveAll(x => x.TripReservationId == id);

            TripReservationDeleted?.Invoke(id);
        }
    }
}
