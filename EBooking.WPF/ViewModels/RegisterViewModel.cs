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
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string employeeFirstName;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string employeeLastName;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string adminDisplayName;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string username;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyPropertyChangedFor(nameof(RepeatPassword))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string password;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyPropertyChangedFor(nameof(Password))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateRepeatPassword))]
        private string repeatPassword;

        // Custom validation
        public static ValidationResult? ValidateRepeatPassword(string password, ValidationContext context)
        {
            var instance = (RegisterViewModel)context.ObjectInstance;
            var isValid = instance.Password == password;

            if (isValid)
                return ValidationResult.Success;
            return new ValidationResult("Passwords must match!");
        }

        public IRelayCommand RegisterCommand { get; }

        private readonly MessageQueueService _messageQueueService;
        private readonly UserService _userService;

        public RegisterViewModel(MessageQueueService messageQueueService, UserService userService)
        {
            _messageQueueService = messageQueueService;
            _userService = userService;
            selectedTabIndex = 0;
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
