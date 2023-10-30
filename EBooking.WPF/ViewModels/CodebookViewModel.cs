using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Dialogs;
using EBooking.WPF.Utility;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace EBooking.WPF.ViewModels
{
    public partial class CodebookViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private ObservableObject currentCodebook;
        [ObservableProperty]
        private int selectedCodebookIndex;
        partial void OnSelectedCodebookIndexChanged(int value)
        {
            if (value == 0)
                CurrentCodebook = _createLocationsViewModel();
        }

        private readonly Func<LocationsViewModel> _createLocationsViewModel;

        public CodebookViewModel(Func<LocationsViewModel> createLocationsViewModel)
        {
            _createLocationsViewModel = createLocationsViewModel;
            currentCodebook = new LocationsViewModel();
            selectedCodebookIndex = 0;
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.CODEBOOK);
    }
}
