using EBooking.WPF.Dialogs.ViewModels;
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

        public static ValidationResult? ValidateAvailableFromDate(DateTime date, ValidationContext context)
        {
            var isValid = date >= DateTime.Now.Date;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Future date required");
        }

        public static ValidationResult? ValidateReservationFromDate(DateTime date, ValidationContext context)
        {
            var isValid = date >= DateTime.Now.Date;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Future date required");
        }

        public static ValidationResult? ValidateAvailableToDateOnAdd(DateTime date, ValidationContext context)
        {
            var instance = (AccommodationUnitAddDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Must be higher than from date");
        }

        public static ValidationResult? ValidateAvailableToDateOnEdit(DateTime date, ValidationContext context)
        {
            var instance = (AccommodationUnitEditDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Must be higher than from date");
        }

        public static ValidationResult? ValidateReservationToDateOnAdd(DateTime date, ValidationContext context)
        {
            var instance = (UnitReservationAddDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.ReservationFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Must be higher than from date");
        }

        public static ValidationResult? ValidateReservationToDateOnEdit(DateTime date, ValidationContext context)
        {
            var instance = (UnitReservationEditDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.ReservationFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Must be higher than from date");
        }

        public static ValidationResult? ValidatePositiveDecimalNumber(string number, ValidationContext context)
        {
            bool isDecimal = decimal.TryParse(number, out var result);
            bool isValid = isDecimal && result >= 0;
            if (!isValid)
                return new ValidationResult("Positive decimal number is required");
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidatePositiveIntegerNumber(string number, ValidationContext context)
        {
            bool isNumber = int.TryParse(number, out var result);
            bool isValid = isNumber && result >= 0;
            if (!isValid)
                return new ValidationResult("Positive integer number is required");
            return ValidationResult.Success;
        }
        #endregion
    }
}
