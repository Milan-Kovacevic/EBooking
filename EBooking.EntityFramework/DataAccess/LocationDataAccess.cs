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
    internal class LocationDataAccess : DataAccessBase, ILocationDAO
    {
        public LocationDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory)
        { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Locations.FirstOrDefaultAsync(x => x.LocationId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.Locations.Remove(entity);

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

        public async Task<IEnumerable<Location>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.Locations.Project().To<Location>().ToListAsync();
            }
        }

        public async Task<Location> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Locations.FirstOrDefaultAsync(x => x.LocationId == id);
                if (entity is null)
                    throw new DataAccessException();

                return Mapper.Map(entity).ToANew<Location>();
            }
        }

        public async Task<Location> Insert(Location entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var location = Mapper.Map(entity).ToANew<LocationEntity>();
                var result = await context.Locations.AddAsync(location);

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

        public async Task Update(Location entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.Locations.FirstOrDefaultAsync(x => x.LocationId == entity.LocationId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result);
                context.Locations.Update(result);

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
