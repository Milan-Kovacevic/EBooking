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
    internal class AccommodationDataAccess : DataAccessBase, IAccommodationDAO
    {
        public AccommodationDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Accommodation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Accommodation> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Accommodation> Insert(Accommodation entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(Accommodation entity)
        {
            throw new NotImplementedException();
        }
    }
}
