using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Dialogs.DialogViewModels;
using EBooking.WPF.Stores;
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
        private readonly LocationsStore _locationsStore;

        public DialogHostService(LocationsStore locationsStore)
        {
            _locationsStore = locationsStore;
        }

        public async Task ShowExitApplicationDialog()
        {
            var dialogContent = new ConfirmExitDialog();
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowConfirmDeleteDialog(Func<Task> onDeleteAction)
        {
            var dialogContent = new ConfirmDeleteDialog();
            dialogContent.OnYesCommand = new AsyncRelayCommand(onDeleteAction);
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowAddLocationDialog(Func<SubmitLocationViewModel, Task> onLocationAddAction)
        {
            var dialogContent = new SubmitLocationDialog(onLocationAddAction);
            dialogContent.DialogTitle.Text = "Create New Location";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowEditLocationDialog(Func<SubmitLocationViewModel, Task> onLocationEditAction, LocationItemViewModel viewModel)
        {
            var dialogContent = new SubmitLocationDialog(onLocationEditAction, viewModel);
            dialogContent.DialogTitle.Text = "Edit Location";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowAddUnitFeatureDialog(Func<SubmitUnitFeatureViewModel, Task> onFeatureAddAction)
        {
            var dialogContent = new SubmitUnitFeatureDialog(onFeatureAddAction);
            dialogContent.DialogTitle.Text = "Create New Unit Feature";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowEditUnitFeatureDialog(Func<SubmitUnitFeatureViewModel, Task> onFeatureEditAction, UnitFeatureItemViewModel viewModel)
        {
            var dialogContent = new SubmitUnitFeatureDialog(onFeatureEditAction, viewModel);
            dialogContent.DialogTitle.Text = "Edit Unit Feature";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowAddAccommodationDialog(Func<SubmitAccommodationViewModel, Task> onAccommodationAddAction)
        {
            var dialogContent = new SubmitAccommodationDialog(_locationsStore, onAccommodationAddAction);
            dialogContent.DialogTitle.Text = "Create New Accommodation";
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public async Task ShowFilterAccommodationsDialog()
        {
            var dialogContent = new FilterAccommodationsDialog();
            await DialogHost.Show(dialogContent, DIALOG_HOST_ROOT_NAME);
        }

        public void CloseDialogHost()
        {
            if (DialogHost.IsDialogOpen(DIALOG_HOST_ROOT_NAME))
                DialogHost.Close(DIALOG_HOST_ROOT_NAME);
        }
    }
}
