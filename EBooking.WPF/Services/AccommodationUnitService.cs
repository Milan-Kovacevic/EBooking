using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class AccommodationUnitService
    {
        private readonly AccommodationUnitStore _accommodationUnitStore;

        public AccommodationUnitService(AccommodationUnitStore accommodationUnitStore)
        {
            _accommodationUnitStore = accommodationUnitStore;
        }

        public Task AddAccommodationUnit(AccommodationUnit accommodationUnit)
        {
            return _accommodationUnitStore.Insert(accommodationUnit);
        }

        public Task UpdateAccommodationUnit(AccommodationUnit accommodationUnit)
        {
            if (_accommodationUnitStore.AccommodationUnits.Any(x => x.UnitId == accommodationUnit.UnitId))
                return _accommodationUnitStore.Update(accommodationUnit);
            return Task.CompletedTask;
        }

        public Task DeleteAccommodationUnit(int accommodationUnitId)
        {
            if (_accommodationUnitStore.AccommodationUnits.Any(x => x.UnitId == accommodationUnitId))
                return _accommodationUnitStore.Delete(accommodationUnitId);
            return Task.CompletedTask;
        }

        public Accommodation? GetCurrentAccommodation()
        {
            return _accommodationUnitStore.CurrentAccommodation;
        }

        public void SetSelectedAccommodationUnit(int unitId)
        {
            var result = _accommodationUnitStore.AccommodationUnits.FirstOrDefault(x => x.UnitId == unitId);
            _accommodationUnitStore.SelectedAccommodationUnit = result;
        }
        public AccommodationUnit? GetSelectedAccommodationUnit()
        {
            return _accommodationUnitStore.SelectedAccommodationUnit;
        }
    }
}
