using EBooking.Domain.DAOs;
using EBooking.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class UnitFeaturesStore
    {
        private readonly IUnitFeatureDAO _unitFeatureDao;
        private readonly List<UnitFeature> _unitFeatures;

        public IEnumerable<UnitFeature> UnitFeatures { get => _unitFeatures; }
        public UnitFeature? SelectedUnitFeature { get; set; }
        public event Action? UnitFeatureLoaded;
        public event Action<UnitFeature>? UnitFeatureAdded;
        public event Action<UnitFeature>? UnitFeatureUpdated;
        public event Action<int>? UnitFeatureDeleted;

        public UnitFeaturesStore(IUnitFeatureDAO unitFeatureDao)
        {
            _unitFeatureDao = unitFeatureDao;
            _unitFeatures = new List<UnitFeature>();
        }

        public async Task Load()
        {
            var loadedFeatures = await _unitFeatureDao.GetAll();

            _unitFeatures.Clear();
            _unitFeatures.AddRange(loadedFeatures);

            UnitFeatureLoaded?.Invoke();
        }

        public async Task Insert(UnitFeature feature)
        {
            var result = await _unitFeatureDao.Insert(feature);
            _unitFeatures.Add(result);

            UnitFeatureAdded?.Invoke(result);
        }

        public async Task Update(UnitFeature feature)
        {
            await _unitFeatureDao.Update(feature);
            int featureIndex = _unitFeatures.FindIndex(f => f.FeatureId == feature.FeatureId);
            if (featureIndex == -1)
                _unitFeatures.Add(feature);
            else
                _unitFeatures[featureIndex] = feature;

            UnitFeatureUpdated?.Invoke(feature);
        }

        public async Task Delete(int id)
        {
            await _unitFeatureDao.Delete(id);
            _unitFeatures.RemoveAll(x => x.FeatureId == id);

            UnitFeatureDeleted?.Invoke(id);
        }
    }
}
