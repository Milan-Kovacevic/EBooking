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

        private readonly Dictionary<ThemeColor, ThemeColorPalette> primaryColors;
        private readonly Dictionary<ThemeColor, ThemeColorPalette> secondaryColors;
        public IEnumerable<ColorItem> PrimaryColors 
        { 
            get => primaryColors.Values
                .Select(x => new ColorItem(x.Key, x.Color, Util.GetLocalizedValue(x.Key))); 
        }
        public IEnumerable<ColorItem> SecondaryColors
        {
            get => secondaryColors.Values
                .Select(x => new ColorItem(x.Key, x.Color, Util.GetLocalizedValue(x.Key)));
        }

        public static ThemeProvider Instance { get; } = new ThemeProvider();

        enum ThemeColor
        {
            RED, BLUE, CYAN, GREEN, ORANGE
        }

        private ThemeProvider()
        {
            primaryColors = new Dictionary<ThemeColor, ThemeColorPalette>()
            {
                { ThemeColor.RED, new ThemeColorPalette("color-red", Color.FromRgb(255, 0, 0)) },
                { ThemeColor.GREEN, new ThemeColorPalette("color-green", Color.FromRgb(0, 255, 0)) },
            };

            // TODO
            secondaryColors = new Dictionary<ThemeColor, ThemeColorPalette>()
            {
                { ThemeColor.CYAN, new ThemeColorPalette("color-cyan", Color.FromRgb(100, 20, 60)) },
                { ThemeColor.BLUE, new ThemeColorPalette("color-blue", Color.FromRgb(0, 0, 255)) },
            };
        }

        public void ApplyPrimaryColorThemeChange(Color primaryColor)
        {
            SetUIPrimaryColorTheme(primaryColor);
        }

        public void ApplySecondaryColorThemeChange(Color secondaryColor)
        {
            SetUISecondaryColorTheme(secondaryColor);
        }

        public void ApplyBaseThemeChange(IBaseTheme baseTheme)
        {
            SetUIBaseTheme(baseTheme);
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
