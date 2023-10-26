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
        public record class ThemePalette(Color PrimaryColor, Color SecondaryColor, string Key);

        private readonly List<ThemePalette> themePalettes;
        public IEnumerable<ThemePalette> ThemePalettes { get => themePalettes; }

        public static ThemeProvider Instance { get; } = new ThemeProvider();

        private ThemeProvider()
        {
            themePalettes = new List<ThemePalette>
            {
                new ThemePalette(Color.FromRgb(0, 188, 212), Color.FromRgb(0, 96, 100), "Cyan" ),
                new ThemePalette(Color.FromArgb(255, 100, 100, 100), Color.FromArgb(255, 0, 200, 50), "Red" )
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
