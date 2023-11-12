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
    internal class TripReservationDataAccess : DataAccessBase, ITripReservationDAO
    {
        public TripReservationDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.TripReservations.FirstOrDefaultAsync(x => x.TripReservationId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.TripReservations.Remove(entity);

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

        public async Task<IEnumerable<TripReservation>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.TripReservations
                    .Include(x => x.Employee)
                    .Include(x => x.Flights)
                    .ThenInclude(f => f.Flight)
                    .Select(x => MapTripReservationEntityToDTO(x)).ToListAsync();
            }
        }

        public async Task<TripReservation> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.TripReservations
                    .Include(x => x.Employee)
                    .Include(x => x.Flights)
                    .ThenInclude(f => f.Flight)
                    .FirstOrDefaultAsync(x => x.TripReservationId == id);
                if (entity is null)
                    throw new DataAccessException();

                return MapTripReservationEntityToDTO(entity);
            }
        }

        public async Task<TripReservation> Insert(TripReservation entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var tripReservation = Mapper.Map(entity).ToANew<TripReservationEntity>(cfg => cfg.MapEntityKeys());
                var result = await context.TripReservations.AddAsync(tripReservation);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return await GetById(result.Entity.TripReservationId);
            }
        }

        public async Task Update(TripReservation entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.TripReservations.Include(x => x.Flights).FirstOrDefaultAsync(x => x.TripReservationId == entity.TripReservationId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result, cfg => cfg.MapEntityKeys());
                result.Flights = entity.Flights.Select((x, i) => new FlightOnTripReservationEntity() { FlightId = x.FlightId, TripReservationId = entity.TripReservationId, SerialNumber = i }).ToList();
                context.TripReservations.Update(result);

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

        private TripReservation MapTripReservationEntityToDTO(TripReservationEntity entity)
        {
            var tripReservation = Mapper.Map(entity).ToANew<TripReservation>();
            entity.Flights.Sort((x1, x2) => x1.SerialNumber - x2.SerialNumber);
            tripReservation.Flights = entity.Flights.Select(x => Mapper.Map(x.Flight).ToANew<Flight>()).ToList();
            return tripReservation;
        }
    }
}
