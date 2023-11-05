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
    internal class UserDataAccess : DataAccessBase, IUserDAO
    {
        public UserDataAccess(EBookingDbContextFactory contextFactory) : base(contextFactory)
        { }

        public async Task<bool> Delete(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                if (entity == null)
                    throw new DataAccessException();
                context.Users.Remove(entity);

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

        public async Task<IEnumerable<User>> GetAll()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.Users.Select(x => Mapper.Map(x).ToANew<User>()).ToListAsync();
            }
        }

        public async Task<IEnumerable<Administrator>> GetAllAdministrators()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.Administrators.Project().To<Administrator>().ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                return await context.Employees.Project().To<Employee>().ToListAsync();
            }
        }

        public async Task<User> GetById(int id)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                if (entity is null)
                    throw new DataAccessException();

                return await MapSpecificUser(context, entity);
            }
        }

        public async Task<User> GetByUsername(string username)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var entity = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (entity is null)
                    throw new DataAccessException();
                return await MapSpecificUser(context, entity);
            }
        }

        public async Task<User> Insert(User entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var user = Mapper.Map(entity).ToANew<UserEntity>();
                var result = await context.Users.AddAsync(user);

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

        public async Task Update(User entity)
        {
            using (EBookingDbContext context = _contextFactory.Create())
            {
                var result = await context.Users.FirstOrDefaultAsync(x => x.UserId == entity.UserId);
                if (result == null)
                    throw new DataAccessException();

                Mapper.Map(entity).Over(result);
                context.Users.Update(result);

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

        private static async Task<User> MapSpecificUser(EBookingDbContext context, UserEntity entity)
        {
            if (entity is EmployeeEntity)
            {
                var result = await context.Employees
                    .Include(x=> x.TripReservations)
                    .Include(x=> x.UnitReservations)
                    .FirstOrDefaultAsync(x => x.UserId == entity.UserId);
                return Mapper.Map(entity).ToANew<Employee>();
            }
            else if (entity is AdministratorEntity)
            {
                var result = await context.Administrators
                    .FirstOrDefaultAsync(x => x.UserId == entity.UserId);
                return Mapper.Map(result).ToANew<Administrator>();
            }

            return Mapper.Map(entity).ToANew<User>();
        }
    }
}
