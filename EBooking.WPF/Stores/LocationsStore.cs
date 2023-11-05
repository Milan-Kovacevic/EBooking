using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class LocationsStore
    {
        private readonly ILocationDAO _locationDao;
        private readonly List<Location> _locations;

        public IEnumerable<Location> Locations { get => _locations; }
        public Location? SelectedLocation { get; set; }
        public event Action? LocationLoaded;
        public event Action<Location>? LocationAdded;
        public event Action<Location>? LocationUpdated;
        public event Action<int>? LocationDeleted;

        public LocationsStore(ILocationDAO locationDao)
        {
            _locationDao = locationDao;
            _locations = new List<Location>();
        }

        public async Task Load()
        {
            var loadedLocations = await _locationDao.GetAll();

            _locations.Clear();
            _locations.AddRange(loadedLocations);


            LocationLoaded?.Invoke();
        }

        public async Task Insert(Location location)
        {
            var result = await _locationDao.Insert(location);
            _locations.Add(result);

            LocationAdded?.Invoke(result);
        }

        public async Task Update(Location location)
        {
            await _locationDao.Update(location);
            int locationIndex = _locations.FindIndex(f => f.LocationId == location.LocationId);
            if (locationIndex == -1)
                _locations.Add(location);
            else
                _locations[locationIndex] = location;

            LocationUpdated?.Invoke(location);
        }

        public async Task Delete(int id)
        {
            await _locationDao.Delete(id);
            _locations.RemoveAll(x => x.LocationId == id);

            LocationDeleted?.Invoke(id);
        }
    }
}
