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
            { MessageType.SUCCESSFUL_USER_INFO_CHANGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "SuccessfulUserInfoChange" },
            { MessageType.SUCCESSFUL_SETTINGS_CHANGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "SuccessfulSettingsChange" },
            { MessageType.TRIP_RESERVATION_ADDED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "TripReservationAdded" },
            { MessageType.TRIP_RESERVATION_UPDATED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "TripReservationUpdated" },
            { MessageType.TRIP_RESERVATION_DELETED, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "TripReservationDeleted" },
            { MessageType.PASSWORD_MISSMATCH, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "PasswordMissmatch" },
            { MessageType.USERNAME_TAKEN, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UsernameTaken" },
            { MessageType.ACCOMMODATION_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationAddDialogTitle" },
            { MessageType.ACCOMMODATION_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationEditDialogTitle" },
            { MessageType.ACCOMMODATION_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationDeleteDialogMessage" },
            { MessageType.REQUIRED_PROPERTY_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "RequiredPropertyMessage" },
            { MessageType.ACCOMMODATION_UNIT_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUnitAddDialogTitle" },
            { MessageType.ACCOMMODATION_UNIT_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUnitEditDialogTitle" },
            { MessageType.ACCOMMODATION_UNIT_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "AccommodationUnitDeleteDialogMessage" },
            { MessageType.FUTURE_DATE_REQUIRED_PROPERTY_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "FutureDateRequiredMessage" },
            { MessageType.GREATER_THAN_FROM_DATE_REQUIRED_PROPERTY_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "GreaterThanFromDateRequiredMessage" },
            { MessageType.GREATER_THAN_DEPARTURE_DATE_REQUIRED_PROPERTY_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "GreaterThanDepartureDateRequiredMessage" },
            { MessageType.GREATER_THAN_DEPARTURE_TIME_REQUIRED_PROPERTY_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "GreaterThanDepartureTimeRequiredMessage" },
            { MessageType.POSITIVE_DECIMAL_NUMBER_REQUIRED_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "PositiveDecimalNumberRequiredMessage" },
            { MessageType.POSITIVE_INTEGER_NUMBER_REQUIRED_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "PositiveIntegerNumberRequiredMessage" },
            { MessageType.PASSWORDS_MUST_MATCH_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "PasswordsMustMatchMessage" },
            { MessageType.NOT_SAME_AS_CURRENT_PASSWORD_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "NotSameAsCurrentPasswordMessage" },
            { MessageType.UNIT_RESERVATION_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationAddDialogTitle" },
            { MessageType.UNIT_RESERVATION_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationEditDialogTitle" },
            { MessageType.UNIT_RESERVATION_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationDeleteDialogMessage" },
            { MessageType.FLIGHT_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "FlightAddDialogTitle" },
            { MessageType.FLIGHT_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "FlightEditDialogTitle" },
            { MessageType.FLIGHT_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "FlightDeleteDialogMessage" },
            { MessageType.LOCATION_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "LocationAddDialogTitle" },
            { MessageType.LOCATION_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "LocationEditDialogTitle" },
            { MessageType.LOCATION_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "LocationDeleteDialogMessage" },
            { MessageType.UNIT_FEATURE_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitFeatureAddDialogTitle" },
            { MessageType.UNIT_FEATURE_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitFeatureEditDialogTitle" },
            { MessageType.UNIT_FEATURE_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitFeatureDeleteDialogMessage" },
            { MessageType.TRIP_RESERVATION_ADD_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "TripReservationAddDialogTitle" },
            { MessageType.TRIP_RESERVATION_EDIT_DIALOG_TITLE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "TripReservationEditDialogTitle" },
            { MessageType.TRIP_RESERVATION_DELETE_DIALOG_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "TripReservationDeleteDialogMessage" },
            { MessageType.UNIT_RESERVATION_FROM_DATE_VALIDATION_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationFromDateValidationMessage" },
            { MessageType.UNIT_RESERVATION_TO_DATE_VALIDATION_MESSAGE, GLOBAL_LANGUAGE_DICTIONARY_PREFIX + "UnitReservationToDateValidationMessage" },
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
            SUCCESSFUL_USER_INFO_CHANGE,
            SUCCESSFUL_SETTINGS_CHANGE,
            TRIP_RESERVATION_ADDED,
            TRIP_RESERVATION_UPDATED,
            TRIP_RESERVATION_DELETED,
            PASSWORD_MISSMATCH,
            USERNAME_TAKEN,
            ACCOMMODATION_ADD_DIALOG_TITLE,
            ACCOMMODATION_EDIT_DIALOG_TITLE,
            ACCOMMODATION_DELETE_DIALOG_MESSAGE,
            REQUIRED_PROPERTY_MESSAGE,
            ACCOMMODATION_UNIT_ADD_DIALOG_TITLE,
            ACCOMMODATION_UNIT_EDIT_DIALOG_TITLE,
            ACCOMMODATION_UNIT_DELETE_DIALOG_MESSAGE,
            FUTURE_DATE_REQUIRED_PROPERTY_MESSAGE,
            GREATER_THAN_FROM_DATE_REQUIRED_PROPERTY_MESSAGE,
            GREATER_THAN_DEPARTURE_DATE_REQUIRED_PROPERTY_MESSAGE,
            GREATER_THAN_DEPARTURE_TIME_REQUIRED_PROPERTY_MESSAGE,
            POSITIVE_DECIMAL_NUMBER_REQUIRED_MESSAGE,
            POSITIVE_INTEGER_NUMBER_REQUIRED_MESSAGE,
            PASSWORDS_MUST_MATCH_MESSAGE,
            NOT_SAME_AS_CURRENT_PASSWORD_MESSAGE,
            UNIT_RESERVATION_ADD_DIALOG_TITLE,
            UNIT_RESERVATION_EDIT_DIALOG_TITLE,
            UNIT_RESERVATION_DELETE_DIALOG_MESSAGE,
            FLIGHT_ADD_DIALOG_TITLE,
            FLIGHT_EDIT_DIALOG_TITLE,
            FLIGHT_DELETE_DIALOG_MESSAGE,
            LOCATION_ADD_DIALOG_TITLE,
            LOCATION_EDIT_DIALOG_TITLE,
            LOCATION_DELETE_DIALOG_MESSAGE,
            UNIT_FEATURE_ADD_DIALOG_TITLE,
            UNIT_FEATURE_EDIT_DIALOG_TITLE,
            UNIT_FEATURE_DELETE_DIALOG_MESSAGE,
            TRIP_RESERVATION_ADD_DIALOG_TITLE,
            TRIP_RESERVATION_EDIT_DIALOG_TITLE,
            TRIP_RESERVATION_DELETE_DIALOG_MESSAGE,
            UNIT_RESERVATION_FROM_DATE_VALIDATION_MESSAGE,
            UNIT_RESERVATION_TO_DATE_VALIDATION_MESSAGE,
        }

        public static string Translate(MessageType message)
        {
            var key = _translationKeys[message];
            return Util.GetLocalizedValue(key);
        }
    }
}
