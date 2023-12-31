﻿using CommunityToolkit.Mvvm.Input;
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
        private readonly DialogNavigationService _navigateToAccommodationUnitAddDialogViewModel;
        private readonly DialogNavigationService _navigateToAccommodationUnitDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToAccommodationUnitEditDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitReservationAddDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitReservationEditDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitReservationDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToSaveSettingsDialogViewModel;
        private readonly DialogNavigationService _navigateToFlightAddDialogViewModel;
        private readonly DialogNavigationService _navigateToFlightEditDialogViewModel;
        private readonly DialogNavigationService _navigateToFlightDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToChangePasswordDialogViewModel;
        private readonly DialogNavigationService _navigateToChangeUserInfoDialogViewModel;
        private readonly DialogNavigationService _navigateToTripReservationAddDialogViewModel;
        private readonly DialogNavigationService _navigateToTripReservationEditDialogViewModel;
        private readonly DialogNavigationService _navigateToTripReservationDeleteDialogViewModel;
        private readonly DialogNavigationService _navigateToTripReservationDetailsDialogViewModel;
        private readonly DialogNavigationService _navigateToUnitReservationDetailsDialogViewModel;

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
            DialogNavigationService navigateToAccommodationDeleteDialogViewModel,
            DialogNavigationService navigateToAccommodationUnitAddDialogViewModel,
            DialogNavigationService navigateToAccommodationUnitDeleteDialogViewModel,
            DialogNavigationService navigateToAccommodationUnitEditDialogViewModel,
            DialogNavigationService navigateToUnitReservationAddDialogViewModel,
            DialogNavigationService navigateToUnitReservationEditDialogViewModel,
            DialogNavigationService navigateToUnitReservationDeleteDialogViewModel,
            DialogNavigationService navigateToSaveSettingsDialogViewModel,
            DialogNavigationService navigateToFlightAddDialogViewModel,
            DialogNavigationService navigateToFlightEditDialogViewModel,
            DialogNavigationService navigateToFlightDeleteDialogViewModel,
            DialogNavigationService navigateToChangePasswordDialogViewModel,
            DialogNavigationService navigateToTripReservationAddDialogViewModel,
            DialogNavigationService navigateToTripReservationEditDialogViewModel,
            DialogNavigationService navigateToTripReservationDeleteDialogViewModel,
            DialogNavigationService navigateToChangeUserInfoDialogViewModel,
            DialogNavigationService navigateToTripReservationDetailsDialogViewModel,
            DialogNavigationService navigateToUnitReservationDetailsDialogViewModel)
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
            _navigateToAccommodationUnitAddDialogViewModel = navigateToAccommodationUnitAddDialogViewModel;
            _navigateToAccommodationUnitDeleteDialogViewModel = navigateToAccommodationUnitDeleteDialogViewModel;
            _navigateToAccommodationUnitEditDialogViewModel = navigateToAccommodationUnitEditDialogViewModel;
            _navigateToUnitReservationAddDialogViewModel = navigateToUnitReservationAddDialogViewModel;
            _navigateToUnitReservationEditDialogViewModel = navigateToUnitReservationEditDialogViewModel;
            _navigateToUnitReservationDeleteDialogViewModel = navigateToUnitReservationDeleteDialogViewModel;
            _navigateToSaveSettingsDialogViewModel = navigateToSaveSettingsDialogViewModel;
            _navigateToFlightAddDialogViewModel = navigateToFlightAddDialogViewModel;
            _navigateToFlightEditDialogViewModel = navigateToFlightEditDialogViewModel;
            _navigateToFlightDeleteDialogViewModel = navigateToFlightDeleteDialogViewModel;
            _navigateToChangePasswordDialogViewModel = navigateToChangePasswordDialogViewModel;
            _navigateToTripReservationAddDialogViewModel = navigateToTripReservationAddDialogViewModel;
            _navigateToTripReservationEditDialogViewModel = navigateToTripReservationEditDialogViewModel;
            _navigateToTripReservationDeleteDialogViewModel = navigateToTripReservationDeleteDialogViewModel;
            _navigateToChangeUserInfoDialogViewModel = navigateToChangeUserInfoDialogViewModel;
            _navigateToTripReservationDetailsDialogViewModel = navigateToTripReservationDetailsDialogViewModel;
            _navigateToUnitReservationDetailsDialogViewModel = navigateToUnitReservationDetailsDialogViewModel;
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

        public void OpenAccommodationUnitAddDialog()
        {
            _navigateToAccommodationUnitAddDialogViewModel.Navigate();
        }

        public void OpenAccommodationUnitEditDialog()
        {
            _navigateToAccommodationUnitEditDialogViewModel.Navigate();
        }

        public void OpenAccommodationUnitDeleteDialog()
        {
            _navigateToAccommodationUnitDeleteDialogViewModel.Navigate();
        }

        public void OpenUnitReservationAddDialog()
        {
            _navigateToUnitReservationAddDialogViewModel.Navigate();
        }

        public void OpenUnitReservationEditDialog()
        {
            _navigateToUnitReservationEditDialogViewModel.Navigate();
        }

        public void OpenUnitReservationDeleteDialog()
        {
            _navigateToUnitReservationDeleteDialogViewModel.Navigate();
        }

        public void OpenFlightAddDialog()
        {
            _navigateToFlightAddDialogViewModel.Navigate();
        }

        public void OpenFlightEditDialog()
        {
            _navigateToFlightEditDialogViewModel.Navigate();
        }

        public void OpenFightDeleteDialog()
        {
            _navigateToFlightDeleteDialogViewModel.Navigate();
        }

        public void OpenSaveSettingsDialog()
        {
            _navigateToSaveSettingsDialogViewModel.Navigate();
        }

        public void OpenChangePasswordDialog()
        {
            _navigateToChangePasswordDialogViewModel.Navigate();
        }

        public void OpenChangeUserInfoDialog()
        {
            _navigateToChangeUserInfoDialogViewModel.Navigate();
        }

        public void OpenTripReservationAddDialog()
        {
            _navigateToTripReservationAddDialogViewModel.Navigate();
        }

        public void OpenTripReservationEditDialog()
        {
            _navigateToTripReservationEditDialogViewModel.Navigate();
        }

        public void OpenTripReservationDeleteDialog()
        {
            _navigateToTripReservationDeleteDialogViewModel.Navigate();
        }

        public void OpenTripReservationDetailsDialog()
        {
            _navigateToTripReservationDetailsDialogViewModel.Navigate();
        }

        public void OpenUnitReservationDetailsDialog()
        {
            _navigateToUnitReservationDetailsDialogViewModel.Navigate();
        }

        public void CloseDialogHost()
        {
            if (DialogHost.IsDialogOpen(DIALOG_HOST_ROOT_NAME))
                DialogHost.Close(DIALOG_HOST_ROOT_NAME);
        }
    }
}
