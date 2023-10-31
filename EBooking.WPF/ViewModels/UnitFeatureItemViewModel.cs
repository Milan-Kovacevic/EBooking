using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class UnitFeatureItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private int featureId;
        [ObservableProperty]
        private string name;

        public UnitFeatureItemViewModel()
        {
            isSelected = false;
            featureId = 0;
            name = string.Empty;
        }
    }
}
