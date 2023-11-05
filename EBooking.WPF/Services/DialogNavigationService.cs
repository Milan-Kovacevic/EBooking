using EBooking.WPF.Stores;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class DialogNavigationService
    {
        private readonly DialogNavigationStore _dialogNavigationStore;
        private readonly Func<IViewModelBase> createDialogViewModel;

        public DialogNavigationService(DialogNavigationStore dialogNavigationStore, Func<IViewModelBase> createDialogViewModel)
        {
            _dialogNavigationStore = dialogNavigationStore;
            this.createDialogViewModel = createDialogViewModel;
        }

        public void Navigate()
        {
            if (_dialogNavigationStore.CurrentDialogViewModel is null)
                _dialogNavigationStore.CurrentDialogViewModel = createDialogViewModel();
            else if (_dialogNavigationStore.CurrentDialogViewModel.CanNavigateFrom())
                _dialogNavigationStore.CurrentDialogViewModel = createDialogViewModel();
        }

        public bool CanNavigate()
        {
            bool? canNavigate = _dialogNavigationStore.CurrentDialogViewModel?.CanNavigateFrom();
            return canNavigate is not null && canNavigate == true;
        }
    }
}
