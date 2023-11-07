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
            var instance = (AccommodationUnitAddDialogViewModel)context.ObjectInstance;
            var isValid = date > instance.AvailableFrom;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Must be higher than from date");
        }

        public static ValidationResult? ValidateNumberOfBeds(string number, ValidationContext context)
        {
            if(!int.TryParse(number, out var result))
                return new ValidationResult("Positive number is required");

            var isValid = result >= 0 && result <= 20;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Number of beds can be between 0 and 20");
        }

        public static ValidationResult? ValidatePricePerNight(string price, ValidationContext context)
        {
            if (!decimal.TryParse(price, out var result))
                return new ValidationResult("Positive price is required");
            var isValid = result >= 0.0m;
            if (isValid)
                return ValidationResult.Success;

            return new ValidationResult("Invalid price specified");
        }
        #endregion
    }
}
