using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class AccommodationUnitItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private int unitId;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private DateTime availableFrom;
        [ObservableProperty]
        private DateTime availableTo;
        [ObservableProperty]
        private int numberOfBeds;
        [ObservableProperty]
        private decimal pricePerNight;
        [ObservableProperty]
        private decimal? unitSize;

        public AccommodationUnitItemViewModel()
        {
            unitId = 0;
            name = string.Empty;
            availableFrom = DateTime.Now; 
            availableTo = DateTime.Now;
            numberOfBeds = 0;
            pricePerNight = 0.0m;
            unitSize = null;
        }

    }
}
