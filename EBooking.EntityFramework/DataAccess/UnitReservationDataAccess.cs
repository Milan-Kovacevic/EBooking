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
    internal class UnitReservationDataAccess : DataAccessBase, IUnitReservationDAO
    {
        public UnitReservationDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.UnitReservations.FirstOrDefaultAsync(x => x.UnitReservationId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.UnitReservations.Remove(entity);

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

        public async Task<IEnumerable<UnitReservation>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.UnitReservations
                    .Include(x => x.Employee)
                    .Include(x => x.Unit)
                    .Select(x => Mapper.Map(x).ToANew<UnitReservation>()).ToListAsync();
            }
        }

        public async Task<UnitReservation> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.UnitReservations
                    .Include(x => x.Employee)
                    .Include(x => x.Unit)
                    .FirstOrDefaultAsync(x => x.UnitReservationId == id);
                if (entity is null)
                    throw new DataAccessException();

                return Mapper.Map(entity).ToANew<UnitReservation>();
            }
        }

        public async Task<UnitReservation> Insert(UnitReservation entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var unitReservation = Mapper.Map(entity).ToANew<AccommodationUnitReservationEntity>(cfg => cfg.MapEntityKeys());
                var result = await context.UnitReservations.AddAsync(unitReservation);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return await GetById(result.Entity.UnitReservationId);
            }
        }

        public async Task<IEnumerable<UnitReservation>> LoadReservationsForAccommodation(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.UnitReservations
                    .Include(x => x.Employee)
                    .Include(x => x.Unit)
                    .Where(x=> x.Unit.AccommodationId == id)
                    .Select(x => Mapper.Map(x).ToANew<UnitReservation>()).ToListAsync();
            }
        }

        public async Task Update(UnitReservation entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.UnitReservations.FirstOrDefaultAsync(x => x.UnitReservationId == entity.UnitReservationId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result, cfg => cfg.MapEntityKeys());
                context.UnitReservations.Update(result);

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
