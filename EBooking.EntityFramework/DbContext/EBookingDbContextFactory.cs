using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DbContext
{
    internal class EBookingDbContextFactory
    {
        public required string ConnectionString { get; init; }

        public EBookingDbContext Create()
        {
            var options = new DbContextOptionsBuilder().UseSqlite(ConnectionString).Options;
            return new EBookingDbContext(options);
        }
    }
}
