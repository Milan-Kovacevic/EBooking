using EBooking.WPF.Models;
using EBooking.WPF.Utility;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class SettingsStore
    {
        private ApplicationSettings? _currentSettings;
        public ApplicationSettings CurrentSettings { get => _currentSettings ?? _internalBackupSettings; }
        public bool IsSettingsLoaded { get => _currentSettings != null; }

        public SettingsStore()
        {}

        public void LoadSettings()
        {
            var settings = Properties.Settings.Default;
            BaseTheme baseTheme = (BaseTheme)settings.BaseTheme;

            IBaseTheme theme;
            if (baseTheme == BaseTheme.Inherit)
                baseTheme = Theme.GetSystemTheme() ?? BaseTheme.Light;


            if (baseTheme == BaseTheme.Light)
                theme = Theme.Light;
            else
                theme = Theme.Dark;


            ApplicationSettings appSettings = new ApplicationSettings()
            {
                LanguageCode = settings.LanguageCode,
                BaseTheme = theme,
                PrimaryColor = Util.ConvertDrawingColorToMediaColor(settings.PrimaryColor),
                SecondaryColor = Util.ConvertDrawingColorToMediaColor(settings.SecondaryColor),
            };
            _currentSettings = appSettings;
        }

        public void SaveSettings()
        {
            if (CurrentSettings == null)
                return;

            var settings = Properties.Settings.Default;
            BaseTheme theme;
            if (CurrentSettings.BaseTheme == Theme.Light)
                theme = BaseTheme.Light;
            else
                theme = BaseTheme.Dark;

            settings.LanguageCode = CurrentSettings.LanguageCode;
            settings.BaseTheme = (int)theme;
            settings.PrimaryColor = Util.ConvertMediaColorToDrawingColor(CurrentSettings.PrimaryColor);
            settings.SecondaryColor = Util.ConvertMediaColorToDrawingColor(CurrentSettings.SecondaryColor);
            Properties.Settings.Default.Save();
        }


        #region Internal Backing Settings
        /// <summary>
        /// Provides internal instance to be used if loading of application settings fails
        /// </summary>
        private static readonly ApplicationSettings _internalBackupSettings = new ApplicationSettings()
        {
            LanguageCode = "en-US",
            BaseTheme = Theme.Dark,
            PrimaryColor = ThemeProvider.Instance.PrimaryColors.ElementAt(0).Value,
            SecondaryColor = ThemeProvider.Instance.SecondaryColors.ElementAt(0).Value,
        };
        #endregion
    }
}
