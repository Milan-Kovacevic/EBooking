using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DbContext
{
    internal class EBookingDesignTimeDbContext : IDesignTimeDbContextFactory<EBookingDbContext>
    {
        // Used only in design time to create migrations upon ORM. Ignored in run time...
        public EBookingDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder().UseSqlite("Data Source=ebooking.db").Options;
            return new EBookingDbContext(options);
        }
    }
}
