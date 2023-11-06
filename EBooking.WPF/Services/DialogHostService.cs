using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.Dialogs;
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
        private readonly DialogNavigationService _navigateToExitApplicationDialogViewModel;
        private readonly DialogNavigationService _navigateToLocationDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToLocationAddDialogViewModel;
        private readonly DialogNavigationService _navigateToLocationEditDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitFeatureDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitFeatureAddDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitFeatureEditDialogViewModel;
        private readonly DialogNavigationService _navigateToMultiDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToAccommodatioAddDialogViewModel;
        private readonly DialogNavigationService _navigateToAccommodationEditDialogViewModel;
        private readonly DialogNavigationService _navigateToAccommodationDeleteDialogViewModel;

        public DialogHostService(
            DialogNavigationService navigateToExitApplicationDialogViewModel,
            DialogNavigationService navigateToLocationDeleteDialogViewModel,
            DialogNavigationService navigateToLocationAddDialogViewModel,
            DialogNavigationService navigateToLocationEditDialogViewModel,
            DialogNavigationService navigateToUnitFeatureDeleteDialogViewModel,
            DialogNavigationService navigateToUnitFeatureAddDialogViewModel,
            DialogNavigationService navigateToUnitFeatureEditDialogViewModel,
            DialogNavigationService navigateToMultiDeleteDialogViewModel,
            DialogNavigationService navigateToAccommodatioAddDialogViewModel,
            DialogNavigationService navigateToAccommodationEditDialogViewModel,
            DialogNavigationService navigateToAccommodationDeleteDialogViewModel)
        {
            _navigateToExitApplicationDialogViewModel = navigateToExitApplicationDialogViewModel;
            _navigateToLocationDeleteDialogViewModel = navigateToLocationDeleteDialogViewModel;
            _navigateToLocationAddDialogViewModel = navigateToLocationAddDialogViewModel;
            _navigateToLocationEditDialogViewModel = navigateToLocationEditDialogViewModel;
            _navigateToUnitFeatureDeleteDialogViewModel = navigateToUnitFeatureDeleteDialogViewModel;
            _navigateToUnitFeatureAddDialogViewModel = navigateToUnitFeatureAddDialogViewModel;
            _navigateToUnitFeatureEditDialogViewModel = navigateToUnitFeatureEditDialogViewModel;
            _navigateToMultiDeleteDialogViewModel = navigateToMultiDeleteDialogViewModel;
            _navigateToAccommodatioAddDialogViewModel = navigateToAccommodatioAddDialogViewModel;
            _navigateToAccommodationEditDialogViewModel = navigateToAccommodationEditDialogViewModel;
            _navigateToAccommodationDeleteDialogViewModel = navigateToAccommodationDeleteDialogViewModel;
        }

        public void OpenExitApplicationDialog()
        {
            _navigateToExitApplicationDialogViewModel.Navigate();
        }

        public void OpenLocationDeleteDialog()
        {
            _navigateToLocationDeleteDialogViewModel.Navigate();
        }

        public void OpenLocationAddDialog()
        {
            _navigateToLocationAddDialogViewModel.Navigate();
        }

        public void OpenLocationEditDialog()
        {
            _navigateToLocationEditDialogViewModel.Navigate();
        }

        public void OpenUnitFeatureDeleteDialog()
        {
            _navigateToUnitFeatureDeleteDialogViewModel.Navigate();
        }

        public void OpenUnitFeatureAddDialog()
        {
            _navigateToUnitFeatureAddDialogViewModel.Navigate();
        }

        public void OpenUnitFeatureEditDialog()
        {
            _navigateToUnitFeatureEditDialogViewModel.Navigate();
        }

        public void OpenConfirmMultiDeleteDialog()
        {
            _navigateToMultiDeleteDialogViewModel.Navigate();
        }

        public void OpenAccommodationDeleteDialog()
        {
            _navigateToAccommodationDeleteDialogViewModel.Navigate();
        }

        public void OpenAccommodationAddDialog()
        {
            _navigateToAccommodatioAddDialogViewModel.Navigate();
        }

        public void OpenAccommodationEditDialog()
        {
            _navigateToAccommodationEditDialogViewModel.Navigate();
        }

        public void CloseDialogHost()
        {
            if (DialogHost.IsDialogOpen(DIALOG_HOST_ROOT_NAME))
                DialogHost.Close(DIALOG_HOST_ROOT_NAME);
        }
    }
}
