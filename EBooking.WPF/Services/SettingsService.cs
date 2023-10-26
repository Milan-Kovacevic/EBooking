using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace EBooking.WPF.Services
{
    public class SettingsService
    {
        private const string APP_DATABASE_CONFIGURATION_KEY = "Database";
        private readonly SettingsStore _settingsStore;

        public SettingsService(SettingsStore settingsStore)
        {
            _settingsStore = settingsStore;
        }

        public void ApplyCurrentSettings()
        {
            var currentSettings = _settingsStore.CurrentSettings;
            if (currentSettings == null)
                return;

            PaletteHelper paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            currentTheme.SetBaseTheme(currentSettings.BaseTheme);
            currentTheme.SetPrimaryColor(currentSettings.PrimaryColor);
            currentTheme.SetSecondaryColor(currentSettings.SecondaryColor);
            paletteHelper.SetTheme(currentTheme);

            var cultureInfo = new CultureInfo(currentSettings.LanguageCode);
            LocalizeDictionary.Instance.Culture = cultureInfo;
        }

        public void ChangeBaseTheme(bool isDarkTheme)
        {
            IBaseTheme baseTheme;
            if (isDarkTheme)
                baseTheme = Theme.Dark;
            else
                baseTheme = Theme.Light;
            ThemeProvider.Instance.ApplyBaseThemeChange(baseTheme);

            if (_settingsStore.IsSettingsLoaded)
                _settingsStore.CurrentSettings.BaseTheme = baseTheme;
        }

        public void ChangePrimaryColor(Color primaryColor)
        {
            var mediaPrimaryColor = Util.ConvertDrawingColorToMediaColor(primaryColor);
            ThemeProvider.Instance.ApplyPrimaryColorThemeChange(mediaPrimaryColor);
            if (_settingsStore.IsSettingsLoaded)
                _settingsStore.CurrentSettings.PrimaryColor = mediaPrimaryColor;
        }

        public void ChangeSecondaryColor(Color secondaryColor)
        {
            var mediaSecondaryColor = Util.ConvertDrawingColorToMediaColor(secondaryColor);
            ThemeProvider.Instance.ApplyPrimaryColorThemeChange(mediaSecondaryColor);
            if (_settingsStore.IsSettingsLoaded)
                _settingsStore.CurrentSettings.PrimaryColor = mediaSecondaryColor;
        }

        public void SaveCurrentSettings()
        {
            _settingsStore.SaveSettings();
        }

        public void RevertSavedSettings()
        {
            _settingsStore.LoadSettings();
            ApplyCurrentSettings();
        }

        public bool IsDarkThemeSet()
        {
            return _settingsStore.CurrentSettings.BaseTheme == Theme.Dark;
        }

        public string? LoadConnectionString()
        {
            var databaseType = ConfigurationManager.AppSettings[APP_DATABASE_CONFIGURATION_KEY];
            if (databaseType is null)
                return null;

            return ConfigurationManager.ConnectionStrings[databaseType].ConnectionString;
        }
    }
}
