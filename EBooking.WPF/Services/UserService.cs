using EBooking.WPF.Stores;
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

        public bool IsLoggedIn()
        {
            return _userStore.IsLoggedIn;
        }

        public void Login()
        {
            _userStore.IsLoggedIn = true;
        }

        public void Logout()
        {
            _userStore.IsLoggedIn = false;
        }
    }
}
