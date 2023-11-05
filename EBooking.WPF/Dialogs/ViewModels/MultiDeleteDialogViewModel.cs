using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EBooking.WPF.Messages;
using EBooking.WPF.Services;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class MultiDeleteDialogViewModel : ObservableObject, IViewModelBase
    {
        private readonly DialogHostService _dialogHostService;

        public MultiDeleteDialogViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
        }

        [RelayCommand]
        public void Cancel()
        {
            _dialogHostService.CloseDialogHost();
        }

        [RelayCommand]
        public void Submit()
        {
            WeakReferenceMessenger.Default.Send(new DeleteSelectedDialogResultMessage(true));
            _dialogHostService.CloseDialogHost();
        }
    }
}
