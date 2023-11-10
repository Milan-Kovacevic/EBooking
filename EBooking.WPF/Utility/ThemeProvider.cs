using EBooking.WPF.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EBooking.WPF.Utility
{
    internal class ThemeProvider
    {
        private record class ThemeColorPalette(string Key, Color Color);

        private readonly Dictionary<string, Color> primaryColors;
        private readonly Dictionary<string, Color> secondaryColors;
        public IEnumerable<ColorItem> PrimaryColors
        {
            get => primaryColors
                .Select(x => new ColorItem(x.Key, x.Value, Util.GetLocalizedValue(x.Key)));
        }
        public IEnumerable<ColorItem> SecondaryColors
        {
            get => secondaryColors
                .Select(x => new ColorItem(x.Key, x.Value, Util.GetLocalizedValue(x.Key)));
        }

        public static ThemeProvider Instance { get; } = new ThemeProvider();

        private ThemeProvider()
        {
            // DESERIALIZE DICTIONARY EXTERNALY OF AVAILABLE THEME COLORS
            primaryColors = new Dictionary<string, Color>()
            {
                { "color-blue", Color.FromRgb(11, 98, 152) },
                { "color-red",Color.FromRgb(190, 49, 68) },
                { "color-teal", Color.FromRgb(0, 102, 102) },
            };

            secondaryColors = new Dictionary<string, Color>()
            {
                { "color-blue", Color.FromRgb(27, 158, 238) },
                { "color-red", Color.FromRgb(220, 121, 134) },
                { "color-teal", Color.FromRgb(100, 204, 197) },
            };
        }

        public void ApplyPrimaryColorThemeChange(string primaryColorKey)
        {
            string key = ResolvePrimaryColorCode(primaryColorKey);
            SetUIPrimaryColorTheme(primaryColors[key]);
        }

        public void ApplySecondaryColorThemeChange(string secondaryColorKey)
        {
            string key = ResolveSecondaryColorCode(secondaryColorKey);
            SetUISecondaryColorTheme(secondaryColors[key]);
        }

        public void ApplyBaseThemeChange(IBaseTheme baseTheme)
        {
            SetUIBaseTheme(baseTheme);
        }

        public string ResolvePrimaryColorCode(string expectedCode)
        {
            string key = expectedCode;
            if (!primaryColors.ContainsKey(expectedCode))
                key = primaryColors.ElementAt(0).Key;

            return key;
        }

        public string ResolveSecondaryColorCode(string expectedCode)
        {
            string key = expectedCode;
            if (!secondaryColors.ContainsKey(expectedCode))
                key = secondaryColors.ElementAt(0).Key;

            return key;
        }

        #region Private Methods
        private void SetUIPrimaryColorTheme(Color primaryColor)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            currentTheme.SetPrimaryColor(primaryColor);
            paletteHelper.SetTheme(currentTheme);
        }

        private void SetUISecondaryColorTheme(Color secondaryColor)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            currentTheme.SetSecondaryColor(secondaryColor);
            paletteHelper.SetTheme(currentTheme);
        }

        private void SetUIBaseTheme(IBaseTheme baseTheme)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            currentTheme.SetBaseTheme(baseTheme);
            paletteHelper.SetTheme(currentTheme);
        }
        #endregion
    }
}
