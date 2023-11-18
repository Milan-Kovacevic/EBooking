using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ItemViewModels
{
    public partial class LocationItemViewModel : ObservableObject
    {
        public int LocationId { get; set; }
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private string country;
        [ObservableProperty]
        private string city;

        public LocationItemViewModel()
        {
            LocationId = 0;
            isSelected = false;
            country = string.Empty;
            city = string.Empty;
        }
    }
}
