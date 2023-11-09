using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class UnitReservationStore
    {
        private readonly IUnitReservationDAO _unitReservationDao;
        private readonly List<UnitReservation> _unitReservations;

        public IEnumerable<UnitReservation> UnitReservations { get => _unitReservations; }
        public UnitReservation? SelectedUnitReservation { get; set; }
        public event Action? UnitReservationLoaded;
        public event Action<UnitReservation>? UnitReservationAdded;
        public event Action<UnitReservation>? UnitReservationUpdated;
        public event Action<int>? UnitReservationDeleted;

        public UnitReservationStore(IUnitReservationDAO unitReservationDao)
        {
            _unitReservationDao = unitReservationDao;
            _unitReservations = new List<UnitReservation>();
        }

        public async Task Load()
        {
            var loadedUnitReservations = await _unitReservationDao.GetAll();
            _unitReservations.Clear();
            _unitReservations.AddRange(loadedUnitReservations);

            UnitReservationLoaded?.Invoke();
        }

        public async Task LoadUnitsForAccommodation(int accommodationId)
        {
            var loadedUnitReservations = await _unitReservationDao.LoadReservationsForAccommodation(accommodationId);
            _unitReservations.Clear();
            _unitReservations.AddRange(loadedUnitReservations);

            UnitReservationLoaded?.Invoke();
        }

        public async Task Insert(UnitReservation unitReservation)
        {
            var result = await _unitReservationDao.Insert(unitReservation);
            _unitReservations.Add(result);

            UnitReservationAdded?.Invoke(result);
        }

        public async Task Update(UnitReservation unitReservation)
        {
            await _unitReservationDao.Update(unitReservation);
            var result = await _unitReservationDao.GetById(unitReservation.UnitReservationId);
            int unitReservationIndex = _unitReservations.FindIndex(f => f.UnitReservationId == result.UnitReservationId);
            if (unitReservationIndex == -1)
                _unitReservations.Add(result);
            else
                _unitReservations[unitReservationIndex] = result;

            UnitReservationUpdated?.Invoke(result);
        }

        public async Task Delete(int id)
        {
            await _unitReservationDao.Delete(id);
            _unitReservations.RemoveAll(x => x.UnitReservationId == id);

            UnitReservationDeleted?.Invoke(id);
        }
    }
}
