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

        public Task AddUnitFeature(string name)
        {
            var feature = new UnitFeature() { Name = name };
            return _unitFeaturesStore.Insert(feature);
        }

        public Task UpdateUnitFeature(int featureId, string name)
        {
            var feature = new UnitFeature() { FeatureId = featureId, Name = name };
            if (_unitFeaturesStore.UnitFeatures.Any(x => x.FeatureId == featureId))
                return _unitFeaturesStore.Update(feature);
            return Task.CompletedTask;
        }

        public Task DeleteUnitFeature(int featureId)
        {
            if (_unitFeaturesStore.UnitFeatures.Any(x => x.FeatureId == featureId))
                return _unitFeaturesStore.Delete(featureId);
            return Task.CompletedTask;
        }

        public void SetSelectedUnitFeature(int featureId)
        {
            var result = _unitFeaturesStore.UnitFeatures.FirstOrDefault(x => x.FeatureId == featureId);
            _unitFeaturesStore.SelectedUnitFeature = result;
        }
        public UnitFeature? GetSelectedUnitFeature()
        {
            return _unitFeaturesStore.SelectedUnitFeature;
        }
        public void ClearSelectedUnitFeature()
        {
            _unitFeaturesStore.SelectedUnitFeature = null;
        }
    }
}
