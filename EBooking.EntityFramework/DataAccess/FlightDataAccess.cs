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
    internal class FlightDataAccess : DataAccessBase, IFlightDAO
    {
        public FlightDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Flight>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Flight> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Flight> Insert(Flight entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(Flight entity)
        {
            throw new NotImplementedException();
        }
    }
}
