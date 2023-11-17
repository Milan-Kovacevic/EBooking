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
    public partial class RegisterViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private int selectedTabIndex;
        partial void OnSelectedTabIndexChanged(int value)
        {
            ClearInputProperties();
            ClearErrors();
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        private string employeeFirstName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        private string employeeLastName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        private string adminDisplayName;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        private string username;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        private string password;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRepeatPasswordOnRegister))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        private string repeatPassword;

        partial void OnPasswordChanged(string value)
        {
            if (RepeatPassword != string.Empty)
                ValidateProperty(RepeatPassword, nameof(RepeatPassword));
        }

        public IRelayCommand RegisterCommand { get; }

        private readonly MessageQueueService _messageQueueService;
        private readonly UserService _userService;

        public RegisterViewModel(MessageQueueService messageQueueService, UserService userService)
        {
            _messageQueueService = messageQueueService;
            _userService = userService;
            selectedTabIndex = 1;
            employeeFirstName = string.Empty;
            employeeLastName = string.Empty;
            adminDisplayName = string.Empty;
            username = string.Empty;
            password = string.Empty;
            repeatPassword = string.Empty;

            RegisterCommand = new AsyncRelayCommand(Register, CanRegister);
        }

        private async Task Register()
        {
            bool isSuccessfull;
            if (IsAdminRegistrationActive)
                isSuccessfull = await _userService.RegisterAsAdministrator(Username, Password, AdminDisplayName);
            else
                isSuccessfull = await _userService.RegisterAsEmployee(Username, Password, EmployeeFirstName, EmployeeLastName);

            if(isSuccessfull)
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.SUCCESSFUL_REGISTRATION);
            else
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.INVALID_REGISTRATION);

        }

        private bool CanRegister()
        {
            return (
                (SelectedTabIndex == 0 && EmployeeFirstName != string.Empty && EmployeeLastName != string.Empty)
                || (SelectedTabIndex == 1 && AdminDisplayName != string.Empty)
                )
                && Username != string.Empty
                && Password != string.Empty
                && RepeatPassword != string.Empty
                && !HasErrors;
        }

        private void ClearInputProperties()
        {
            Username = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
            EmployeeFirstName = string.Empty;
            EmployeeLastName = string.Empty;
            AdminDisplayName = string.Empty;
        }

        private bool IsAdminRegistrationActive => SelectedTabIndex == 1;

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.REGISTER);
    }
}
