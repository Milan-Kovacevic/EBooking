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
    internal class FlightDataAccess : DataAccessBase, IFlightDAO
    {
        public FlightDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory) { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Flights.FirstOrDefaultAsync(x => x.FlightId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.Flights.Remove(entity);

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

        public async Task<IEnumerable<Flight>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.Flights
                    .Include(x => x.FromLocation)
                    .Include(x => x.ToLocation)
                    .Include(x => x.Administrator)
                    .Select(x => Mapper.Map(x).ToANew<Flight>()).ToListAsync();
            }
        }

        public async Task<Flight> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Flights
                    .Include(x => x.FromLocation)
                    .Include(x => x.ToLocation)
                    .Include(x => x.Administrator)
                    .FirstOrDefaultAsync(x => x.FlightId == id);
                if (entity is null)
                    throw new DataAccessException();

                return Mapper.Map(entity).ToANew<Flight>();
            }
        }

        public async Task<Flight> Insert(Flight entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var flight = Mapper.Map(entity).ToANew<FlightEntity>(cfg => cfg.MapEntityKeys());
                var result = await context.Flights.AddAsync(flight);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, ex);
                }

                return await GetById(result.Entity.FlightId);
            }
        }

        public async Task Update(Flight entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.Flights.FirstOrDefaultAsync(x => x.FlightId == entity.FlightId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result, cfg => cfg.MapEntityKeys());
                context.Flights.Update(result);

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
