using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DAOs
{
    public interface IUserDAO : IGenericDAO<User, int>
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<IEnumerable<Administrator>> GetAllAdministrators();
        Task<User> GetByUsername(string username);
    }
}
