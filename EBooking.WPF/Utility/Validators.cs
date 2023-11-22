using EBooking.WPF.Dialogs.ViewModels;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EBooking.WPF.Utility
{
    public static class Validators
    {
        #region Custom Validations

        public static ValidationResult? ValidateRequiredProperty(object value, ValidationContext context)
        {
            if(value == null || value.ToString() == string.Empty)
                return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.REQUIRED_PROPERTY_MESSAGE));

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateAvailableFromDate(DateTime date, ValidationContext context)
        {
            var isValid = date >= DateTime.Now.Date;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.FUTURE_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateFutureDate(DateTime date, ValidationContext context)
        {
            var isValid = date >= DateTime.Now.Date;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.FUTURE_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateReservationFromDateOnAdd(DateTime date, ValidationContext context)
        {
            var instance = (UnitReservationAddDialogViewModel)context.ObjectInstance;
            var isValid = date >= DateTime.Now.Date && date >= instance.SelectedUnit?.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_RESERVATION_FROM_DATE_VALIDATION_MESSAGE));
        }

        public static ValidationResult? ValidateReservationFromDateOnEdit(DateTime date, ValidationContext context)
        {
            var instance = (UnitReservationEditDialogViewModel)context.ObjectInstance;
            var isValid = date >= DateTime.Now.Date && date >= instance.SelectedUnit?.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_RESERVATION_FROM_DATE_VALIDATION_MESSAGE));
        }

        public static ValidationResult? ValidateDepartureDate(DateTime date, ValidationContext context)
        {
            var isValid = date >= DateTime.Now.Date;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.FUTURE_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateAvailableToDateOnAdd(DateTime date, ValidationContext context)
        {
            var instance = (AccommodationUnitAddDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.GREATER_THAN_FROM_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateAvailableToDateOnEdit(DateTime date, ValidationContext context)
        {
            var instance = (AccommodationUnitEditDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.GREATER_THAN_FROM_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateReservationToDateOnAdd(DateTime date, ValidationContext context)
        {
            var instance = (UnitReservationAddDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.ReservationFrom && date <= instance.SelectedUnit?.AvailableTo;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_RESERVATION_TO_DATE_VALIDATION_MESSAGE));
        }

        public static ValidationResult? ValidateReservationToDateOnEdit(DateTime date, ValidationContext context)
        {
            var instance = (UnitReservationEditDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.ReservationFrom && date <= instance.SelectedUnit?.AvailableTo;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.UNIT_RESERVATION_TO_DATE_VALIDATION_MESSAGE));
        }

        public static ValidationResult? ValidateArrivalDateOnAdd(DateTime date, ValidationContext context)
        {
            var instance = (FlightAddDialogViewModel)context.ObjectInstance;
            var isValid = date >= instance.DepartureDate;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.GREATER_THAN_DEPARTURE_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateArrivalDateOnEdit(DateTime date, ValidationContext context)
        {
            var instance = (FlightEditDialogViewModel)context.ObjectInstance;
            var isValid = date >= instance.DepartureDate;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.GREATER_THAN_DEPARTURE_DATE_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateArrivalTimeOnAdd(DateTime time, ValidationContext context)
        {
            var instance = (FlightAddDialogViewModel)context.ObjectInstance;
            var isValid = (instance.DepartureDate?.Date == instance.ArrivalDate?.Date && time > instance.DepartureTime) || (instance.DepartureDate?.Date < instance.ArrivalDate?.Date);
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.GREATER_THAN_DEPARTURE_TIME_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidateArrivalTimeOnEdit(DateTime time, ValidationContext context)
        {
            var instance = (FlightEditDialogViewModel)context.ObjectInstance;
            var isValid = (instance.DepartureDate?.Date == instance.ArrivalDate?.Date && time > instance.DepartureTime) || (instance.DepartureDate?.Date < instance.ArrivalDate?.Date);
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.GREATER_THAN_DEPARTURE_TIME_REQUIRED_PROPERTY_MESSAGE));
        }

        public static ValidationResult? ValidatePositiveDecimalNumber(string number, ValidationContext context)
        {
            bool isDecimal = decimal.TryParse(number, out var result);
            bool isValid = isDecimal && result >= 0;
            if (!isValid)
                return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.POSITIVE_DECIMAL_NUMBER_REQUIRED_MESSAGE));
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidatePositiveIntegerNumber(string number, ValidationContext context)
        {
            bool isNumber = int.TryParse(number, out var result);
            bool isValid = isNumber && result >= 0;
            if (!isValid)
                return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.POSITIVE_INTEGER_NUMBER_REQUIRED_MESSAGE));
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateRepeatPasswordOnChange(string password, ValidationContext context)
        {
            var instance = (ChangePasswordDialogViewModel)context.ObjectInstance;
            bool isValid = instance.NewPassword == password;
            if (!isValid)
                return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.PASSWORDS_MUST_MATCH_MESSAGE));
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateRepeatPasswordOnRegister(string password, ValidationContext context)
        {
            var instance = (RegisterViewModel)context.ObjectInstance;
            bool isValid = instance.Password == password;
            if (!isValid)
                return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.PASSWORDS_MUST_MATCH_MESSAGE));
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateNewPassword(string password, ValidationContext context)
        {
            var instance = (ChangePasswordDialogViewModel)context.ObjectInstance;
            bool isValid = instance.CurrentPassword != password;
            if (!isValid)
                return new ValidationResult(LanguageTranslator.Translate(LanguageTranslator.MessageType.NOT_SAME_AS_CURRENT_PASSWORD_MESSAGE));
            return ValidationResult.Success;
        }
        #endregion
    }
}
