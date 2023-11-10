using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Utility
{
    public static class LanguageTranslator
    {
        private static readonly string GLOBAL_LANGUAGE_DICTIONARY_PREFIX = "GLOBAL_";
        private static Dictionary<MessageType, string> _translationKeys = new Dictionary<MessageType, string>()
        {
            { MessageType.WELCOME_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "WelcomeMessage" },
            { MessageType.SUCCESSFUL_LOGIN, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "SuccessfulLogin" },
            { MessageType.INVALID_LOGIN, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "InvalidLogin" },
            { MessageType.SUCCESSFUL_REGISTRATION, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "SuccessfulRegistration" },
            { MessageType.INVALID_REGISTRATION, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "InvalidRegistration" },
        };

        public enum MessageType
        {
            WELCOME_MESSAGE,
            SUCCESSFUL_LOGIN,
            INVALID_LOGIN,
            SUCCESSFUL_REGISTRATION,
            INVALID_REGISTRATION,
        }

        public static string Translate(MessageType message)
        {
            var key = _translationKeys[message];
            return Util.GetLocalizedValue(key);
        }
    }
}
