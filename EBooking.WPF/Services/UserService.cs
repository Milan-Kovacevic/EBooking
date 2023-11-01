using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class UserService
    {
        private readonly UserStore _userStore;

        public UserService(UserStore userStore)
        {
            _userStore = userStore;
        }

        public Task<bool> RegisterAsAdministrator(string username, string password, string name)
        {
            Administrator admin = new Administrator()
            {
                Username = username.Trim(),
                Password = Util.ComputeHash(password),
                Name = name.Trim()
            };
            return _userStore.Register(admin);
        }

        public Task<bool> RegisterAsEmployee(string username, string password, string firstName, string lastName)
        {
            Employee employee = new Employee()
            {
                Username = username.Trim(),
                Password = Util.ComputeHash(password),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
            };
            return _userStore.Register(employee);
        }


        public async Task<bool> Login(string username, string password)
        {
            User? user = await _userStore.SearchByUsername(username);
            if (user is null)
                return false;

            if (user.Password == Util.ComputeHash(password))
            {
                _userStore.CurrentUser = user;
                return true;
            }
            return false;    
        }

        public void Logout()
        {
            _userStore.CurrentUser = null;
        }

        public bool IsLoggedIn()
        {
            return _userStore.IsLoggedIn;
        }
    }
}
