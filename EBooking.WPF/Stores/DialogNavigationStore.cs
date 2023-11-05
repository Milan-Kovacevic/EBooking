using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class DialogNavigationStore
    {
        private IViewModelBase? _currentDialogViewModel;
        public IViewModelBase? CurrentDialogViewModel
        {
            get => _currentDialogViewModel;
            set
            {
                _currentDialogViewModel?.Dispose();
                _currentDialogViewModel = value;
                CurrentDialogViewModelChanged?.Invoke();
            }
        }

        public event Action? CurrentDialogViewModelChanged;
    }
}
