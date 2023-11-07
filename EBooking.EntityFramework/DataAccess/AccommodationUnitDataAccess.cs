using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Extensions;
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
    internal class AccommodationUnitDataAccess : DataAccessBase, IAccommodationUnitDAO
    {
        public AccommodationUnitDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.AccommodationUnits.FirstOrDefaultAsync(x => x.UnitId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.AccommodationUnits.Remove(entity);

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

        public Task<IEnumerable<AccommodationUnit>> GetAll()
        {
            return Task.FromResult(Enumerable.Empty<AccommodationUnit>());
        }

        public async Task<AccommodationUnit> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.AccommodationUnits
                    .Include(x => x.Features)
                    .ThenInclude(f => f.UnitFeature)
                    .FirstOrDefaultAsync(x => x.UnitId == id);
                if (entity is null)
                    throw new DataAccessException();

                return MapAccommodationUnitEntityToDTO(entity);
            }
        }

        public async Task<AccommodationUnit> Insert(AccommodationUnit entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var accommodation = Mapper.Map(entity).ToANew<AccommodationUnitEntity>(cfg => cfg.MapEntityKeys());
                var result = await context.AccommodationUnits.AddAsync(accommodation);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return await GetById(result.Entity.UnitId);
            }
        }

        public async Task<IEnumerable<AccommodationUnit>> LoadForAccommodation(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var accommodationUnits = await context.AccommodationUnits
                    .Include(x => x.Features)
                    .ThenInclude(f => f.UnitFeature)
                    .Where(x => x.AccommodationId == id)
                    .ToListAsync();

                var result = new List<AccommodationUnit>(accommodationUnits.Select(MapAccommodationUnitEntityToDTO));
                return result;
            }
        }

        public async Task Update(AccommodationUnit entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.AccommodationUnits.Include(x => x.Features).FirstOrDefaultAsync(x => x.UnitId == entity.UnitId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result, cfg => cfg.MapEntityKeys());
                result.Features = entity.Features.Select(x => new FeatureOnUnitEntity() { FeatureId = x.FeatureId, UnitId = entity.UnitId }).ToList();
                context.AccommodationUnits.Update(result);

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

        private AccommodationUnit MapAccommodationUnitEntityToDTO(AccommodationUnitEntity entity)
        {
            var unit = Mapper.Map(entity).ToANew<AccommodationUnit>();
            unit.Features = entity.Features.Select(x => Mapper.Map(x.UnitFeature).ToANew<UnitFeature>()).ToList();
            return unit;
        }
    }
}
