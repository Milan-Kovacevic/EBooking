using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class AccommodationUnitStore
    {
        private readonly List<AccommodationUnit> _accommodationUnits;

        public IEnumerable<AccommodationUnit> AccommodationUnits { get => _accommodationUnits; }
        public Accommodation? SelectedAccommodation { get; set; }
        public event Action? AccommodationUnitLoaded;
        public event Action<AccommodationUnit>? AccommodationUnitAdded;
        public event Action<AccommodationUnit>? AccommodationUnitUpdated;
        public event Action<int>? AccommodationUnitDeleted;

        public AccommodationUnitStore()
        {
            _accommodationUnits = new List<AccommodationUnit>();
        }
    }
}
