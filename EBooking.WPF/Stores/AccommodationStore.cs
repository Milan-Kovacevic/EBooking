using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class AccommodationStore
    {
        private readonly IAccommodationDAO _accommodationDao;
        private readonly List<Accommodation> _accommodations;

        public IEnumerable<Accommodation> Accommodations { get => _accommodations; }
        public Accommodation? SelectedAccommodation { get; set; }
        public event Action? AccommodationLoaded;
        public event Action<Accommodation>? AccommodationAdded;
        public event Action<Accommodation>? AccommodationUpdated;
        public event Action<int>? AccommodationDeleted;

        public AccommodationStore(IAccommodationDAO accommodationDao)
        {
            _accommodationDao = accommodationDao;
            _accommodations = new List<Accommodation>();
        }

        public async Task Load()
        {
            var loadedAccommodations = await _accommodationDao.GetAll();

            _accommodations.Clear();
            _accommodations.AddRange(loadedAccommodations);

            AccommodationLoaded?.Invoke();
        }

        public async Task Insert(Accommodation accommodation)
        {
            var result = await _accommodationDao.Insert(accommodation);
            _accommodations.Add(result);

            AccommodationAdded?.Invoke(result);
        }

        public async Task Update(Accommodation accommodation)
        {
            await _accommodationDao.Update(accommodation);
            var result = await _accommodationDao.GetById(accommodation.AccommodationId);
            int accommodationIndex = _accommodations.FindIndex(f => f.AccommodationId == result.AccommodationId);
            if (accommodationIndex == -1)
                _accommodations.Add(result);
            else
                _accommodations[accommodationIndex] = result;

            AccommodationUpdated?.Invoke(result);
        }

        public async Task Delete(int id)
        {
            await _accommodationDao.Delete(id);
            _accommodations.RemoveAll(x => x.AccommodationId == id);

            AccommodationDeleted?.Invoke(id);
        }
    }
}
