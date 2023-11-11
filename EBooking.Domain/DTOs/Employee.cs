using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DTOs
{
    public class Employee : User
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
