using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Exceptions;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class ChangeUserInfoDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string employeeFirstName = string.Empty;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string employeeLastName = string.Empty;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string adminName = string.Empty;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string username = string.Empty;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string confirmPassword = string.Empty;

        public bool IsAdmin { get; set; }

        public IRelayCommand SubmitCommand { get; }

        private readonly UserStore _userStore;
        private readonly UserService _userService;
        private readonly DialogHostService _dialogHostService;
        private readonly MessageQueueService _messageQueueService;

        public ChangeUserInfoDialogViewModel(UserStore userStore, UserService userService, DialogHostService dialogHostService, MessageQueueService messageQueueService)
        {
            _userStore = userStore;
            _userService = userService;
            _dialogHostService = dialogHostService;
            _messageQueueService = messageQueueService;

            IsAdmin = _userStore.IsAdmin;
            var currUser = _userStore.CurrentUser;
            if (currUser is Employee employee)
            {
                employeeFirstName = employee?.FirstName ?? string.Empty;
                employeeLastName = employee?.LastName ?? string.Empty;
            }
            else if(currUser is Administrator admin)
            {
                adminName = admin?.Name ?? string.Empty;
            }
            username = currUser?.Username ?? string.Empty;
            confirmPassword = string.Empty;
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return ((!IsAdmin && EmployeeFirstName != string.Empty
                && EmployeeFirstName != string.Empty)
                || (IsAdmin && AdminName != string.Empty))
                && Username != string.Empty
                && ConfirmPassword != string.Empty
                && !HasErrors;
        }

        private async Task Submit()
        {
            try
            {
                if (IsAdmin)
                    await _userService.UpdateAdministratorInformations(AdminName, Username, ConfirmPassword);
                else
                    await _userService.UpdateEmployeeInformations(EmployeeFirstName, EmployeeLastName, Username, ConfirmPassword);
                _dialogHostService.CloseDialogHost();
            }
            catch (UsernameAlreadyTakenException)
            {
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.USERNAME_TAKEN);
            }
            catch (PasswordMissmatchException)
            {
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.PASSWORD_MISSMATCH);
            }
        }
    }
}
