using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.Converters
{
    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Location location)
            {
                return $"{location.Country}, {location.City}";
            }
            return string.Empty;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            string[] parts = stringValue.Split(new[] { ", " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                return new Location { Country = parts[0], City = parts[1] };
            }

            return null;
        }
    }
}
