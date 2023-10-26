using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace EBooking.WPF.Utility
{
    internal static class Util
    {
        public static System.Drawing.Color ConvertMediaColorToDrawingColor(System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ConvertDrawingColorToMediaColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static string GetLocalizedValue(string key)
        {
            return LocalizeDictionary.Instance
                .GetLocalizedObject("EBooking.WPF", "Language", key, LocalizeDictionary.Instance.Culture)?.ToString() 
                ?? key;
        }
    }
}
