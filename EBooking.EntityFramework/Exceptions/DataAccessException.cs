using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.Exceptions
{
    public class DataAccessException : ApplicationException
    {
        public DataAccessException() : base() { }

        public DataAccessException(string? message) : base(message) { }

        public DataAccessException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
