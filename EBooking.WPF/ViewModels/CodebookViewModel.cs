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
        public LocationsViewModel LocationsViewModel { get; }
        public UnitFeaturesViewModel UnitFeaturesViewModel { get; }

        [ObservableProperty]
        private int selectedCodebookIndex;

        public CodebookViewModel(LocationsViewModel locationsViewModel, UnitFeaturesViewModel unitFeaturesViewModel)
        {
            LocationsViewModel = locationsViewModel;
            UnitFeaturesViewModel = unitFeaturesViewModel;
        }

        public void Dispose()
        {
            LocationsViewModel.Dispose();
            UnitFeaturesViewModel.Dispose();
        }

        public string GetId() => MenuProvider.GetCode(MenuProvider.MenuItem.CODEBOOK);
    }
}
