using EBooking.WPF.Stores;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<IViewModelBase> createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<IViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }

        public void Navigate()
        {
            if(_navigationStore.CurrentViewModel.CanNavigateFrom())
                _navigationStore.CurrentViewModel = createViewModel();
        }
    }
}
