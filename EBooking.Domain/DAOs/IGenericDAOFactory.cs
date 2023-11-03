using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DAOs
{
    public interface IGenericDAOFactory
    {
        public IUserDAO UserDao { get; }
        public ILocationDAO LocationDao { get; }
        public IUnitFeatureDAO UnitFeatureDao { get; }
        public IAccommodationDAO AccommodationDao { get; }
        public IFlightDAO FlightDao { get; }
        public IUnitReservationDAO UnitReservationDao { get; }
        public ITripReservationDAO TripReservationDao { get; }
        
    }
}
