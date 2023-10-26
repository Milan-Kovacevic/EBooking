using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace EBooking.WPF.Utility
{
    internal class LanguageProvider
    {
        private readonly Dictionary<Language, string> applicationLanguages;
        public IEnumerable<string> Languages { get => applicationLanguages.Values.ToList(); }

        private Language currentLanguage;
        public string CurrentLanguage { get => applicationLanguages[currentLanguage]; }

        public static LanguageProvider Instance { get; } = new LanguageProvider();

        public enum Language
        {
            ENGLISH_US, SERBIAN_LATIN, SERBIAN_CYRIL
        }

        private LanguageProvider()
        {
            applicationLanguages = new Dictionary<Language, string>
            {
                { Language.ENGLISH_US, "en-US" },
                { Language.SERBIAN_LATIN, "sr-Latn" },
                { Language.SERBIAN_CYRIL, "sr-Cyrl" }
            };

            currentLanguage = applicationLanguages.Keys.ToList()[0];
        }

        /// <summary>
        /// Saves language change to application settings and updates the UI Language
        /// </summary>
        /// <param name="language"></param>
        public void ApplyLanguageChange(string language)
        {
            var lang = ResolveLanguageCode(language);
            SetUILanguage(lang);
            currentLanguage = lang;
        }

        #region Private Methods
        private void SetUILanguage(Language language)
        {
            var cultureInfo = new CultureInfo(applicationLanguages[language]);
            LocalizeDictionary.Instance.Culture = cultureInfo;
        }
        private Language ResolveLanguageCode(string languageCode)
        {
            return applicationLanguages.FirstOrDefault(x => x.Value.Equals(languageCode)).Key;
        }
        #endregion
    }
}
