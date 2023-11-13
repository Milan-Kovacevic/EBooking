using AgileObjects.AgileMapper;
using EBooking.Domain.DTOs;
using EBooking.WPF.Exceptions;
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

        public async Task<bool> UpdatePassword(string oldPassword, string newPassword)
        {
            var user = _userStore.CurrentUser;
            if (user == null)
                return false;
            if (Util.ComputeHash(oldPassword) != user.Password)
                return false;
            user.Password = Util.ComputeHash(newPassword);
            await _userStore.Update(user);
            return true;
        }

        public async Task UpdateEmployeeInformations(string firstName, string lastName, string username, string confirmPassword)
        {
            var currUser = _userStore.CurrentUser;
            if (currUser is not Employee employee)
                return;

            if (employee.Password != Util.ComputeHash(confirmPassword))
                throw new PasswordMissmatchException();
            var foundUser = await _userStore.SearchByUsername(username);
            if (foundUser is not null && foundUser?.UserId != employee.UserId)
                throw new UsernameAlreadyTakenException();

            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Username = username;
            await _userStore.Update(employee);
        }

        public async Task UpdateAdministratorInformations(string name, string username, string confirmPassword)
        {
            var currUser = _userStore.CurrentUser;
            if (currUser is not Administrator admin)
                return;

            if (admin.Password != Util.ComputeHash(confirmPassword))
                throw new PasswordMissmatchException();
            var foundUser = await _userStore.SearchByUsername(username);
            if (foundUser is not null && foundUser?.UserId != admin.UserId)
                throw new UsernameAlreadyTakenException();

            admin.Name = name;
            admin.Username = username;
            await _userStore.Update(admin);
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
