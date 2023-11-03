using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class LocationsService
    {
        private readonly LocationsStore _locationsStore;

        public LocationsService(LocationsStore locationsStore)
        {
            _locationsStore = locationsStore;
        }

        public Task AddLocation(string countryName, string cityName)
        {
            var location = new Location() { Country = countryName, City = cityName };
            return _locationsStore.Insert(location);
        }

        public Task UpdateLocation(int locationId, string countryName, string cityName)
        {
            if (!_locationsStore.Locations.Any(x => x.LocationId == locationId)) 
                return Task.CompletedTask;
            var location = new Location() { LocationId = locationId, Country = countryName, City = cityName };
            return _locationsStore.Update(location);
            
        }

        public Task DeleteLocation(int locationId)
        {
            if (_locationsStore.Locations.Any(x => x.LocationId == locationId))
                return _locationsStore.Delete(locationId);
            return Task.CompletedTask;
        }
    }
}
