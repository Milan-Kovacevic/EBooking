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

        public Task AddLocation(Location location)
        {
            return _locationsStore.Insert(location);
        }

        public Task UpdateLocation(Location location)
        {
            if (_locationsStore.Locations.Any(x => x.LocationId == location.LocationId))
                return _locationsStore.Update(location);
            return Task.CompletedTask;
        }

        public Task DeleteLocation(int locationId)
        {
            if (_locationsStore.Locations.Any(x => x.LocationId == locationId))
                return _locationsStore.Delete(locationId);
            return Task.CompletedTask;
        }
    }
}
