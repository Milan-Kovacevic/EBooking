using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DAOs
{
    public interface IUnitReservationDAO : IGenericDAO<UnitReservation, int>
    {
        Task<IEnumerable<UnitReservation>> LoadReservationsForAccommodation(int id);
    }
}
