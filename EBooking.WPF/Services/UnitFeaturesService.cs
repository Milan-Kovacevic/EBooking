using EBooking.Domain.DTOs;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class UnitFeaturesService
    {
        private readonly UnitFeaturesStore _unitFeaturesStore;

        public UnitFeaturesService(UnitFeaturesStore unitFeaturesStore)
        {
            _unitFeaturesStore = unitFeaturesStore;
        }

        public Task AddUnitFeature(UnitFeature feature)
        {
            return _unitFeaturesStore.Insert(feature);
        }

        public Task UpdateUnitFeature(UnitFeature feature)
        {
            if (_unitFeaturesStore.UnitFeatures.Any(x => x.FeatureId == feature.FeatureId))
                return _unitFeaturesStore.Update(feature);
            return Task.CompletedTask;
        }

        public Task DeleteUnitFeature(int featureId)
        {
            if (_unitFeaturesStore.UnitFeatures.Any(x => x.FeatureId == featureId))
                return _unitFeaturesStore.Delete(featureId);
            return Task.CompletedTask;
        }
    }
}
