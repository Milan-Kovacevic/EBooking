using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Dialogs.DialogViewModels;
using EBooking.WPF.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class DialogHostService
    {
        private static readonly string DIALOG_HOST_ROOT_NAME = "RootDialog";

        public async Task ShowExitApplicationDialog()
        {
            var dialogContent = new ConfirmExitDialog();
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowConfirmDeleteDialog(Action onDeleteAction)
        {
            var dialogContent = new ConfirmDeleteDialog();
            dialogContent.OnYesCommand = new RelayCommand(onDeleteAction);
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowAddLocationDialog(Action<SubmitLocationViewModel> onLocationAddAction)
        {
            var dialogContent = new SubmitLocationDialog(onLocationAddAction);
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowEditLocationDialog(Action<SubmitLocationViewModel> onLocationEditAction, LocationItemViewModel viewModel)
        {
            var dialogContent = new SubmitLocationDialog(onLocationEditAction, viewModel);
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowAddUnitFeatureDialog(Action<SubmitUnitFeatureViewModel> onFeatureAddAction)
        {
            var dialogContent = new SubmitUnitFeatureDialog(onFeatureAddAction);
            dialogContent.DialogTitle.Text = "Create New Unit Feature";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowEditUnitFeatureDialog(Action<SubmitUnitFeatureViewModel> onFeatureEditAction, UnitFeatureItemViewModel viewModel)
        {
            var dialogContent = new SubmitUnitFeatureDialog(onFeatureEditAction, viewModel);
            dialogContent.DialogTitle.Text = "Edit Unit Feature";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public void CloseDialogHost()
        {
            if (DialogHost.IsDialogOpen(DIALOG_HOST_ROOT_NAME))
                DialogHost.Close(DIALOG_HOST_ROOT_NAME);
        }
    }
}
