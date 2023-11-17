using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
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
    public partial class ChangePasswordDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string currentPassword;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateNewPassword))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string newPassword;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRepeatPasswordOnChange))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string repeatNewPassword;

        partial void OnNewPasswordChanged(string value)
        {
            ValidateProperty(RepeatNewPassword, nameof(RepeatNewPassword));
        }

        partial void OnCurrentPasswordChanged(string value)
        {
            ValidateProperty(NewPassword, nameof(NewPassword));
        }

        private readonly UserService _userService;
        private readonly DialogHostService _dialogHostService;
        private readonly MessageQueueService _messageQueueService;
        public IRelayCommand SubmitCommand { get; }

        public ChangePasswordDialogViewModel(UserService userService, DialogHostService dialogHostService, MessageQueueService messageQueueService)
        {
            _userService = userService;
            _dialogHostService = dialogHostService;
            _messageQueueService = messageQueueService;

            currentPassword = string.Empty;
            newPassword = string.Empty;
            repeatNewPassword = string.Empty;
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return CurrentPassword != string.Empty
                && NewPassword != string.Empty
                && RepeatNewPassword != string.Empty
                && !HasErrors;
        }

        private async Task Submit()
        {
            var isSuccessful = await _userService.UpdatePassword(CurrentPassword, NewPassword);
            if (!isSuccessful)
                _messageQueueService.Enqueue(LanguageTranslator.MessageType.INVALID_PASSWORD_CHANGE);
            else
                _dialogHostService.CloseDialogHost();
        }
    }
}
