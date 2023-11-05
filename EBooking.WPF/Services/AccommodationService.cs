using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class AccommodationService
    {
        private readonly AccommodationStore _accommodationStore;

        public AccommodationService(AccommodationStore accommodationStore)
        {
            _accommodationStore = accommodationStore;
        }

        public Task AddAccommodation(Accommodation accommodation)
        {
            return _accommodationStore.Insert(accommodation);
        }

        public Task UpdateAccommodation(Accommodation accommodation)
        {
            if (_accommodationStore.Accommodations.Any(x => x.AccommodationId == accommodation.AccommodationId))
                return _accommodationStore.Update(accommodation);
            return Task.CompletedTask;
        }

        public Task DeleteAccommodation(int accommodationId)
        {
            if (_accommodationStore.Accommodations.Any(x => x.AccommodationId == accommodationId))
                return _accommodationStore.Delete(accommodationId);
            return Task.CompletedTask;
        }
    }
}
