using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class AccommodationUnitStore
    {
        private readonly IAccommodationUnitDAO _accommodationUnitDao;
        private readonly List<AccommodationUnit> _accommodationUnits;

        public IEnumerable<AccommodationUnit> AccommodationUnits { get => _accommodationUnits; }
        private Accommodation? _currentAccommodation;
        public Accommodation? CurrentAccommodation
        {
            get => _currentAccommodation;
            set
            {
                _currentAccommodation = value;
                LoadUnitsForCurrentAccommodation();
            }
        }

        public AccommodationUnit? SelectedAccommodationUnit { get; set; }
        public event Action? AccommodationUnitLoaded;
        public event Action<AccommodationUnit>? AccommodationUnitAdded;
        public event Action<AccommodationUnit>? AccommodationUnitUpdated;
        public event Action<int>? AccommodationUnitDeleted;

        public AccommodationUnitStore(IAccommodationUnitDAO accommodationUnitDao)
        {
            _accommodationUnitDao = accommodationUnitDao;
            _accommodationUnits = new List<AccommodationUnit>();
        }

        public async void LoadUnitsForCurrentAccommodation()
        {
            if (CurrentAccommodation == null)
                return;

            var loadedUnits = await _accommodationUnitDao.LoadForAccommodation(CurrentAccommodation.AccommodationId);
            _accommodationUnits.Clear();
            _accommodationUnits.AddRange(loadedUnits);
            AccommodationUnitLoaded?.Invoke();
        }

        public async Task Insert(AccommodationUnit accommodationUnit)
        {
            var result = await _accommodationUnitDao.Insert(accommodationUnit);
            _accommodationUnits.Add(result);

            AccommodationUnitAdded?.Invoke(result);
        }

        public async Task Update(AccommodationUnit accommodationUnit)
        {
            await _accommodationUnitDao.Update(accommodationUnit);
            var result = await _accommodationUnitDao.GetById(accommodationUnit.UnitId);
            int accommodationUnitIndex = _accommodationUnits.FindIndex(f => f.UnitId == result.UnitId);
            if (accommodationUnitIndex == -1)
                _accommodationUnits.Add(result);
            else
                _accommodationUnits[accommodationUnitIndex] = result;

            AccommodationUnitUpdated?.Invoke(result);
        }

        public async Task Delete(int id)
        {
            await _accommodationUnitDao.Delete(id);
            _accommodationUnits.RemoveAll(x => x.UnitId == id);

            AccommodationUnitDeleted?.Invoke(id);
        }

    }
}
