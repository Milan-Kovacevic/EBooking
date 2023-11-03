using AgileObjects.AgileMapper;
using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using EBooking.EntityFramework.DbContext;
using EBooking.EntityFramework.Entities;
using EBooking.EntityFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DataAccess
{
    internal class UnitFeatureDataAccess : DataAccessBase, IUnitFeatureDAO
    {
        public UnitFeatureDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory)
        { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.UnitFeatures.FirstOrDefaultAsync(x => x.FeatureId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.UnitFeatures.Remove(entity);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return true;
            }
        }

        public async Task<IEnumerable<UnitFeature>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.UnitFeatures.Project().To<UnitFeature>().ToListAsync();
            }
        }

        public async Task<UnitFeature> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.UnitFeatures.FirstOrDefaultAsync(x => x.FeatureId == id);
                if (entity is null)
                    throw new DataAccessException();

                return Mapper.Map(entity).ToANew<UnitFeature>();
            }
        }

        public async Task<UnitFeature> Insert(UnitFeature entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var feature = Mapper.Map(entity).ToANew<UnitFeatureEntity>();
                var result = await context.UnitFeatures.AddAsync(feature);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return Mapper.Map(result.Entity).Over(entity);
            }
        }

        public async Task Update(UnitFeature entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.UnitFeatures.FirstOrDefaultAsync(x => x.FeatureId == entity.FeatureId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result);
                context.UnitFeatures.Update(result);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }
            }
        }
    }
}
