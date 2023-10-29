using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class LoginViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string username;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password;

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand RegisterCommand { get; }

        private readonly NavigationService _navigateToRegisterViewModel;
        private readonly NavigationService _navigateToLandingViewModel;
        private readonly UserService _userService;

        public LoginViewModel(UserService userService, NavigationService navigateToRegisterViewModel, NavigationService navigateToLandingViewModel)
        {
            _userService = userService;
            _navigateToRegisterViewModel = navigateToRegisterViewModel;
            _navigateToLandingViewModel = navigateToLandingViewModel;
            username = string.Empty;
            password = string.Empty;

            LoginCommand = new RelayCommand(Login, CanLogin);
            RegisterCommand = new RelayCommand(Register);
        }

        private void Login()
        {
            _userService.Login();
            _navigateToLandingViewModel.Navigate();
        }
        
        private bool CanLogin()
        {
            return Username != string.Empty 
                && Password != string.Empty
                && !_userService.IsLoggedIn()
                && !HasErrors;
        }

        private void Register()
        {
            _navigateToRegisterViewModel.Navigate();
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.LOGIN);
    }
}
