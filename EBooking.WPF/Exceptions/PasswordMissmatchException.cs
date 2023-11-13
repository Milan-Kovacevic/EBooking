using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Exceptions
{
    public class PasswordMissmatchException : ApplicationException
    {
        public PasswordMissmatchException() : base() { }

        public PasswordMissmatchException(string? message) : base(message) { }

        public PasswordMissmatchException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
