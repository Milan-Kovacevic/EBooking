using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Services;
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

        private readonly ObservableCollection<UnitFeatureItemViewModel> _unitFeatures;
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

        private readonly DialogHostService _dialogHostService;

        public UnitFeaturesViewModel(DialogHostService dialogHostService)
        {
            _dialogHostService = dialogHostService;
            searchText = string.Empty;
            _unitFeatures = new ObservableCollection<UnitFeatureItemViewModel>()
            {
                new UnitFeatureItemViewModel(){FeatureId = 1, Name ="Free WIFI"},
                new UnitFeatureItemViewModel(){FeatureId = 2, Name ="Kitchen"},
                new UnitFeatureItemViewModel(){FeatureId = 3, Name ="Balcony"}
            };
            UnitFeatures = CollectionViewSource.GetDefaultView(_unitFeatures);
            UnitFeatures.Filter = FilterUnitFeatures;
        }

        public void Dispose() { }

        #region Unit Features CRUD Commands

        [RelayCommand]
        public void SearchUnitFeatures()
        {
            try
            {
                UnitFeatures.Refresh();
            }
            catch
            {

            }
        }

        [RelayCommand]
        public async Task AddUnitFeature()
        {
            await _dialogHostService.ShowAddUnitFeatureDialog((featureVM) =>
            {
                _unitFeatures.Add(new UnitFeatureItemViewModel() { Name = featureVM.FeatureName, FeatureId = _unitFeatures.Count + 1 });
                _dialogHostService.CloseDialogHost();
            });
        }

        [RelayCommand]
        public async Task DeleteUnitFeature(object param)
        {
            if (param is not UnitFeatureItemViewModel vm)
                return;
            await _dialogHostService.ShowConfirmDeleteDialog(() =>
            {
                _unitFeatures.Remove(vm);
                _dialogHostService.CloseDialogHost();
            });
        }

        [RelayCommand]
        public async Task DeleteSelectedFeatures()
        {
            await _dialogHostService.ShowConfirmDeleteDialog(() =>
            {
                _dialogHostService.CloseDialogHost();
            });
        }

        [RelayCommand]
        public async Task EditUnitFeature(object param)
        {
            if (param is not UnitFeatureItemViewModel vm)
                return;
            await _dialogHostService.ShowEditUnitFeatureDialog((featureVM) =>
            {
                var element = _unitFeatures.First(x => x.FeatureId == vm.FeatureId);
                element.Name = featureVM.FeatureName;
                UnitFeatures.Refresh();
                _dialogHostService.CloseDialogHost();
            }, vm);
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
