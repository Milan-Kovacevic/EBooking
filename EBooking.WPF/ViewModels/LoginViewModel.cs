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
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyDataErrorInfo]
        private string username;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyDataErrorInfo]
        private string password;

        [ObservableProperty]
        private bool isLoading;

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand RegisterCommand { get; }

        private readonly NavigationService _navigateToRegisterViewModel;
        private readonly NavigationService _navigateToLandingViewModel;
        private readonly UserService _userService;
        private readonly MessageQueueService _messageQueueService;

        public LoginViewModel(MessageQueueService messageQueueService, UserService userService, NavigationService navigateToRegisterViewModel, NavigationService navigateToLandingViewModel)
        {
            _userService = userService;
            _messageQueueService = messageQueueService;
            _navigateToRegisterViewModel = navigateToRegisterViewModel;
            _navigateToLandingViewModel = navigateToLandingViewModel;
            username = string.Empty;
            password = string.Empty;
            isLoading = false;

            LoginCommand = new AsyncRelayCommand(Login, CanLogin);
            RegisterCommand = new RelayCommand(Register);
        }

        private async Task Login()
        {
            IsLoading = true;
            await Task.Delay(1000);
            bool isSuccessfull = await _userService.Login(Username, Password);
            if (isSuccessfull)
            {
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.SUCCESSFUL_LOGIN);
                _navigateToLandingViewModel.Navigate();
            }   
            else
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.INVALID_LOGIN);
            IsLoading = false;
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
