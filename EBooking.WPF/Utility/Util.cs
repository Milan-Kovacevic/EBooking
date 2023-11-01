using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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

        public static string ComputeHash(string value)
        {
            using (var myHash = SHA256.Create())
            {
                var byteArray = Encoding.UTF8.GetBytes(value);
                var hashedResult = myHash.ComputeHash(byteArray);

                string result = string.Concat(Array.ConvertAll(hashedResult, h => h.ToString("X2")));
                return result;
            }
        }
    }
}
