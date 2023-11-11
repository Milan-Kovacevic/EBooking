﻿using System;
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
            { MessageType.ACCOMMODATION_ADDED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationAdded" },
            { MessageType.ACCOMMODATION_UPDATED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUpdated" },
            { MessageType.ACCOMMODATION_DELETED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationDeleted" },
            { MessageType.ACCOMMODATION_UNIT_ADDED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUnitAdded" },
            { MessageType.ACCOMMODATION_UNIT_UPDATED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUnitUpdated" },
            { MessageType.ACCOMMODATION_UNIT_DELETED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUnitDeleted" },
            { MessageType.UNIT_RESERVATION_ADDED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationAdded" },
            { MessageType.UNIT_RESERVATION_UPDATED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationUpdated" },
            { MessageType.UNIT_RESERVATION_DELETED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationDeleted" },
            { MessageType.INVALID_PASSWORD_CHANGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "InvalidPasswordChange" },
            { MessageType.SUCCESSFUL_PASSWORD_CHANGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "SuccessfulPasswordChange" },
            { MessageType.SUCCESSFUL_SETTINGS_CHANGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "SuccessfulSettingsChange" },
        };

        public enum MessageType
        {
            WELCOME_MESSAGE,
            SUCCESSFUL_LOGIN,
            INVALID_LOGIN,
            SUCCESSFUL_REGISTRATION,
            INVALID_REGISTRATION,
            ACCOMMODATION_ADDED,
            ACCOMMODATION_UPDATED,
            ACCOMMODATION_DELETED,
            ACCOMMODATION_UNIT_ADDED,
            ACCOMMODATION_UNIT_UPDATED,
            ACCOMMODATION_UNIT_DELETED,
            UNIT_RESERVATION_ADDED,
            UNIT_RESERVATION_UPDATED,
            UNIT_RESERVATION_DELETED,
            INVALID_PASSWORD_CHANGE,
            SUCCESSFUL_PASSWORD_CHANGE,
            SUCCESSFUL_SETTINGS_CHANGE,
        }

        public static string Translate(MessageType message)
        {
            var key = _translationKeys[message];
            return Util.GetLocalizedValue(key);
        }
    }
}
