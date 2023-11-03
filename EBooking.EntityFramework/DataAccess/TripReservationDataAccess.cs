using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using EBooking.EntityFramework.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DataAccess
{
    internal class TripReservationDataAccess : DataAccessBase, ITripReservationDAO
    {
        public TripReservationDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TripReservation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TripReservation> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TripReservation> Insert(TripReservation entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(TripReservation entity)
        {
            throw new NotImplementedException();
        }
    }
}
