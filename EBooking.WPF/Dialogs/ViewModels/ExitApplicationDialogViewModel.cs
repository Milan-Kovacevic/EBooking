using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Services;
using EBooking.WPF.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class ExitApplicationDialogViewModel : ObservableObject, IViewModelBase
    {
        private readonly DialogHostService _dialogHostService;

        public ExitApplicationDialogViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
        }

        [RelayCommand]
        public void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        [RelayCommand]
        public void CloseDialog()
        {
            _dialogHostService.CloseDialogHost();
        }
    }
}
