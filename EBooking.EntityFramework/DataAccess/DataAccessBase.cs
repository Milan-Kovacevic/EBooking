using EBooking.EntityFramework.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DataAccess
{
    internal abstract class DataAccessBase
    {
        protected readonly EBookingDbContextFactory _contextFactory;

        public DataAccessBase(EBookingDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
