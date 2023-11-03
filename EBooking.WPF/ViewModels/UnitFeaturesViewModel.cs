using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Dialogs.DialogViewModels;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.ViewModels
{
    public partial class UnitFeaturesViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string searchText;
        partial void OnSearchTextChanged(string value)
        {
            if (value != null && value != string.Empty)
                return;
            SearchUnitFeatures();
        }

        private ObservableCollection<UnitFeatureItemViewModel> _unitFeatures;
        public ICollectionView UnitFeatures { get; }
        public bool? IsAllItemsSelected
        {
            get
            {
                var selected = _unitFeatures.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value);
                    OnPropertyChanged();
                }
            }
        }

        private readonly UnitFeaturesStore _unitFeaturesStore;
        private readonly UnitFeaturesService _unitFeaturesService;
        private readonly DialogHostService _dialogHostService;

        public UnitFeaturesViewModel(UnitFeaturesStore unitFeaturesStore, UnitFeaturesService unitFeaturesService, DialogHostService dialogHostService)
        {
            _unitFeaturesStore = unitFeaturesStore;
            _unitFeaturesStore.UnitFeatureAdded += OnUnitFeatureAdded;
            _unitFeaturesStore.UnitFeatureUpdated += OnUnitFeatureUpdated;
            _unitFeaturesStore.UnitFeatureDeleted += OnUnitFeatureDeleted;
            _unitFeaturesService = unitFeaturesService;
            _dialogHostService = dialogHostService;
            searchText = string.Empty;
            _unitFeatures = new ObservableCollection<UnitFeatureItemViewModel>(unitFeaturesStore.UnitFeatures.Select(x => Mapper.Map(x).ToANew<UnitFeatureItemViewModel>()));
            UnitFeatures = CollectionViewSource.GetDefaultView(_unitFeatures);
            UnitFeatures.Filter = FilterUnitFeatures;
        }

        public void Dispose()
        {
            _unitFeaturesStore.UnitFeatureAdded -= OnUnitFeatureAdded;
            _unitFeaturesStore.UnitFeatureUpdated -= OnUnitFeatureUpdated;
            _unitFeaturesStore.UnitFeatureDeleted -= OnUnitFeatureDeleted;
        }

        #region Unit Features CRUD Commands

        private void OnUnitFeatureAdded(UnitFeature feature)
        {
            _unitFeatures.Add(Mapper.Map(feature).ToANew<UnitFeatureItemViewModel>());
        }
        private void OnUnitFeatureUpdated(UnitFeature feature)
        {
            var featureItemVm = _unitFeatures.FirstOrDefault(f => f.FeatureId == feature.FeatureId);
            Mapper.Map(feature).Over(featureItemVm);
        }
        private void OnUnitFeatureDeleted(int id)
        {
            var featureItemVm = _unitFeatures.FirstOrDefault(f => f.FeatureId == id);

            if (featureItemVm is not null)
                _unitFeatures.Remove(featureItemVm);
        }

        [RelayCommand]
        public void SearchUnitFeatures()
        {
            try
            {
                UnitFeatures.Refresh();
            }
            catch
            { }
        }

        [RelayCommand]
        public async Task AddUnitFeature()
        {
            await _dialogHostService.ShowAddUnitFeatureDialog(AddUnitFeatureAction);
        }
        private async Task AddUnitFeatureAction(SubmitUnitFeatureViewModel featureVM)
        {
            await _unitFeaturesService.AddUnitFeature(new UnitFeature() { Name = featureVM.FeatureName });
            _dialogHostService.CloseDialogHost();
        }

        [RelayCommand]
        public async Task DeleteUnitFeature(object param)
        {
            if (param is not UnitFeatureItemViewModel vm)
                return;
            await _dialogHostService.ShowConfirmDeleteDialog(async () => await DeleteUnitFeatureAction(vm.FeatureId));
        }
        private async Task DeleteUnitFeatureAction(int id)
        {
            await _unitFeaturesService.DeleteUnitFeature(id);
            _dialogHostService.CloseDialogHost();
        }

        [RelayCommand]
        public async Task DeleteSelectedFeatures()
        {
            await _dialogHostService.ShowConfirmDeleteDialog(async () =>
            {
                List<Task> tasks = new List<Task>();
                for (int i = _unitFeatures.Count - 1; i >= 0; i--)
                {
                    if (_unitFeatures[i].IsSelected)
                    {
                        tasks.Add(_unitFeaturesService.DeleteUnitFeature(_unitFeatures[i].FeatureId));
                    }
                }
                await Task.WhenAll(tasks);
                _dialogHostService.CloseDialogHost();
                IsAllItemsSelected = false;
            });
        }

        [RelayCommand]
        public async Task EditUnitFeature(object param)
        {
            if (param is not UnitFeatureItemViewModel vm)
                return;
            await _dialogHostService.ShowEditUnitFeatureDialog(EditUnitFeatureAction, vm);
        }
        private async Task EditUnitFeatureAction(SubmitUnitFeatureViewModel featureVM)
        {
            await _unitFeaturesService.UpdateUnitFeature(new UnitFeature() { Name = featureVM.FeatureName });
            _dialogHostService.CloseDialogHost();
        }

        #endregion
        #region ViewModel Helper Functions
        private bool FilterUnitFeatures(object obj)
        {
            if (obj is null || obj is not UnitFeatureItemViewModel vm)
                return false;
            if (SearchText == null)
                return true;
            return vm.Name.ToLower().Contains(SearchText.ToLower());
        }

        private void SelectAll(bool select)
        {
            foreach (var item in UnitFeatures)
            {
                if (item is UnitFeatureItemViewModel vm)
                {
                    if (FilterUnitFeatures(vm))
                        vm.IsSelected = select;
                }
            }
        }
        #endregion
    }
}
