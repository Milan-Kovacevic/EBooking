using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EBooking.WPF.Models
{
    public class ApplicationSettings
    {
        public required string LanguageCode { get; set; }
        public required IBaseTheme BaseTheme { get; set; }
        public required string PrimaryColorCode { get; set; }
        public required string SecondaryColorCode { get; set; }
    }
}
