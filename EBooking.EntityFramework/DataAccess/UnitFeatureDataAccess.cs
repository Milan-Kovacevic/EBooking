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
    internal class UnitFeatureDataAccess : DataAccessBase, IUnitFeatureDao
    {
        public UnitFeatureDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory)
        {}

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UnitFeature>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UnitFeature> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UnitFeature> Insert(UnitFeature entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(UnitFeature entity)
        {
            throw new NotImplementedException();
        }
    }
}
