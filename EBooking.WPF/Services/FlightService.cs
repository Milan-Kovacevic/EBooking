using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class FlightService
    {
        private readonly FlightStore _flightStore;

        public FlightService(FlightStore flightStore)
        {
            _flightStore = flightStore;
        }

        public Task AddFlight(Flight flight)
        {
            return _flightStore.Insert(flight);
        }

        public Task UpdateFlight(Flight flight)
        {
            if (_flightStore.Flights.Any(x => x.FlightId == flight.FlightId))
                return _flightStore.Update(flight);
            return Task.CompletedTask;
        }

        public Task DeleteFlight(int flightId)
        {
            if (_flightStore.Flights.Any(x => x.FlightId == flightId))
                return _flightStore.Delete(flightId);
            return Task.CompletedTask;
        }

        public void SetSelectedFlight(int flightId)
        {
            var result = _flightStore.Flights.FirstOrDefault(x => x.FlightId == flightId);
            _flightStore.SelectedFlight = result;
        }
        public Flight? GetSelectedFlight()
        {
            return _flightStore.SelectedFlight;
        }
    }
}
