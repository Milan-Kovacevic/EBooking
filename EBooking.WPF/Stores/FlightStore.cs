using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class FlightStore
    {
        private readonly IFlightDAO _flightDao;
        private readonly List<Flight> _flights;

        public IEnumerable<Flight> Flights { get => _flights; }
        public Flight? SelectedFlight { get; set; }
        public event Action? FlightLoaded;
        public event Action<Flight>? FlightAdded;
        public event Action<Flight>? FlightUpdated;
        public event Action<int>? FlightDeleted;

        public FlightStore(IFlightDAO flightDao)
        {
            _flightDao = flightDao;
            _flights = new List<Flight>();
        }

        public async Task Load()
        {
            var loadedFlights = await _flightDao.GetAll();

            _flights.Clear();
            _flights.AddRange(loadedFlights);

            FlightLoaded?.Invoke();
        }

        public async Task Insert(Flight flight)
        {
            var result = await _flightDao.Insert(flight);
            _flights.Add(result);

            FlightAdded?.Invoke(result);
        }

        public async Task Update(Flight flight)
        {
            await _flightDao.Update(flight);
            var result = await _flightDao.GetById(flight.FlightId);
            int flightIndex = _flights.FindIndex(f => f.FlightId == result.FlightId);
            if (flightIndex == -1)
                _flights.Add(result);
            else
                _flights[flightIndex] = result;

            FlightUpdated?.Invoke(result);
        }

        public async Task Delete(int id)
        {
            await _flightDao.Delete(id);
            _flights.RemoveAll(x => x.FlightId == id);

            FlightDeleted?.Invoke(id);
        }
    }
}
