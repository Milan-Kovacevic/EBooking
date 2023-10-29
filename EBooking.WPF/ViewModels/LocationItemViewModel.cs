using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class LocationItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private int locationId;
        [ObservableProperty]
        private string country;
        [ObservableProperty]
        private string city;

        public LocationItemViewModel()
        {
            isSelected = false;
            locationId = 0;
            country = string.Empty;
            city = string.Empty;
        }
    }
}
