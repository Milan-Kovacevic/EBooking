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
    internal class AccommodationDataAccess : DataAccessBase, IAccommodationDAO
    {
        public AccommodationDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Accommodations.FirstOrDefaultAsync(x => x.AccommodationId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.Accommodations.Remove(entity);

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

        public async Task<IEnumerable<Accommodation>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.Accommodations
                    .Include(x => x.Location)
                    .Include(x => x.Administrator)
                    .Include(x => x.AccommodationUnits)
                    .Select(x => Mapper.Map(x).ToANew<Accommodation>()).ToListAsync();
            }
        }

        public async Task<Accommodation> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Accommodations
                    .Include(x => x.Location)
                    .Include(x => x.Administrator)
                    .Include(x => x.AccommodationUnits)
                    .ThenInclude(u => u.Features)
                    .FirstOrDefaultAsync(x => x.AccommodationId == id);
                if (entity is null)
                    throw new DataAccessException();

                return Mapper.Map(entity).ToANew<Accommodation>();
            }
        }

        public async Task<Accommodation> Insert(Accommodation entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var accommodation = Mapper.Map(entity).ToANew<AccommodationEntity>(cfg => cfg.MapEntityKeys());
                var result = await context.Accommodations.AddAsync(accommodation);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return await GetById(result.Entity.AccommodationId);
            }
        }

        public async Task Update(Accommodation entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.Accommodations.FirstOrDefaultAsync(x => x.AccommodationId == entity.AccommodationId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result, cfg => cfg.MapEntityKeys());
                context.Accommodations.Update(result);

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
