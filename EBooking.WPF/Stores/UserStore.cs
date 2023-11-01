using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using EBooking.EntityFramework.Exceptions;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class UserStore
    {
        private readonly IUserDAO _userDao;
        private List<User> _users;
        public IEnumerable<User> Users { get => _users; }

        private User? _user;
        public User? CurrentUser
        {
            get => _user;
            set
            {
                _isLoggedIn = value is not null;
                _user = value;
                CurrentUserChanged?.Invoke();
            }
        }
        public event Action? CurrentUserChanged;

        public UserStore(IUserDAO userDao)
        {
            _userDao = userDao;
            _users = new List<User>();
            _user = null;
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
        }
        public bool IsAdmin { get => IsLoggedIn && CurrentUser is Administrator; }
        public bool IsEmployee { get => IsLoggedIn && CurrentUser is Employee; }


        public async Task Load()
        {
            var loadedUsers = await _userDao.GetAll();

            _users.Clear();
            _users.AddRange(loadedUsers);
        }

        public async Task<User?> SearchByUsername(string username)
        {
            try
            {
                return await _userDao.GetByUsername(username);
            }
            catch (DataAccessException)
            {
                return null;
            }
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                var newUser = await _userDao.Insert(user);
                _users.Add(newUser);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
