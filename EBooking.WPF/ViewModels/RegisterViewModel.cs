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
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string password;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string repeatPassword;

        public IRelayCommand RegisterCommand { get; }

        private readonly MessageQueueService _messageQueueService;

        public RegisterViewModel(MessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
            selectedTabIndex = 0;
            employeeFirstName = string.Empty;
            employeeLastName = string.Empty;
            adminDisplayName = string.Empty;
            username = string.Empty;
            password = string.Empty;
            repeatPassword = string.Empty;

            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        private void Register()
        {
            _messageQueueService.Enqueue("Registration was successfull!");
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

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.REGISTER);
    }
}
