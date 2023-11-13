using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Exceptions
{
    public class UsernameAlreadyTakenException : ApplicationException
    {
        public UsernameAlreadyTakenException() : base() { }

        public UsernameAlreadyTakenException(string? message) : base(message) { }

        public UsernameAlreadyTakenException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
