using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.Dialogs.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class AccommodationUnitEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        public int UnitId { get; set; }
        [ObservableProperty]
        private string dialogTitle;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string name;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateReservationFromDate))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? availableFrom;
        partial void OnAvailableFromChanged(DateTime? value)
        {
            if (AvailableTo is not null)
                ValidateProperty(AvailableTo, nameof(AvailableTo));
        }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateAvailableToDateOnEdit))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private DateTime? availableTo;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveIntegerNumber))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string numberOfBeds;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePositiveDecimalNumber))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string pricePerNight;

        [ObservableProperty]
        private UnitFeatureModel? selectedUnitFeature;
        [ObservableProperty]
        private int selectedAddedFeatureIndex;

        public IRelayCommand SubmitCommand { get; }
        public IEnumerable<UnitFeatureModel> AvailableUnitFeatures { get; }
        private readonly ObservableCollection<UnitFeatureModel> _addedUnitFeatures;
        public ListCollectionView AddedUnitFeatures { get; }

        private readonly UnitFeaturesStore _unitFeaturesStore;
        private readonly AccommodationUnitService _accommodationUnitService;
        private readonly DialogHostService _dialogHostService;


        public AccommodationUnitEditDialogViewModel(UnitFeaturesStore unitFeaturesStore, AccommodationUnitService accommodationUnitService, DialogHostService dialogHostService)
        {
            _unitFeaturesStore = unitFeaturesStore;
            _accommodationUnitService = accommodationUnitService;
            _dialogHostService = dialogHostService;
            AvailableUnitFeatures = _unitFeaturesStore.UnitFeatures.Select(x => Mapper.Map(x).ToANew<UnitFeatureModel>());
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.ACCOMMODATION_UNIT_EDIT_DIALOG_TITLE);
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

            var accommodationUnit = accommodationUnitService.GetSelectedAccommodationUnit();
            _addedUnitFeatures = new ObservableCollection<UnitFeatureModel>(accommodationUnit?.Features.Select(x => Mapper.Map(x).ToANew<UnitFeatureModel>()) ?? Array.Empty<UnitFeatureModel>());
            AddedUnitFeatures = new ListCollectionView(_addedUnitFeatures);
            name = accommodationUnit?.Name ?? string.Empty;
            availableFrom = accommodationUnit?.AvailableFrom ?? null;
            availableTo = accommodationUnit?.AvailableTo ?? null;
            numberOfBeds = accommodationUnit?.NumberOfBeds.ToString() ?? string.Empty;
            pricePerNight = accommodationUnit?.PricePerNight.ToString() ?? string.Empty;
            UnitId = accommodationUnit?.UnitId ?? 0;
            selectedUnitFeature = null;
            selectedAddedFeatureIndex = -1;
        }

        private bool CanSubmit()
        {
            return Name != string.Empty
                && AvailableFrom != null
                && AvailableTo != null
                && int.TryParse(NumberOfBeds, out var _)
                && decimal.TryParse(PricePerNight, out var _)
            && !HasErrors;
        }

        private async Task Submit()
        {
            var accommodationId = _accommodationUnitService.GetCurrentAccommodation()?.AccommodationId ?? 0;
            var features = new List<UnitFeature>(_addedUnitFeatures.Select(x => Mapper.Map(x).ToANew<UnitFeature>()));
            await _accommodationUnitService.UpdateAccommodationUnit(new AccommodationUnit()
            {
                UnitId = UnitId,
                AccommodationId = accommodationId,
                Name = Name,
                AvailableFrom = AvailableFrom ?? DateTime.Now,
                AvailableTo = AvailableTo ?? DateTime.Now,
                NumberOfBeds = int.Parse(NumberOfBeds),
                PricePerNight = decimal.Parse(PricePerNight),
                Features = features
            });
            _dialogHostService.CloseDialogHost();
        }

        [RelayCommand]
        public void AddFeature()
        {
            if (SelectedUnitFeature != null && !_addedUnitFeatures.Contains(SelectedUnitFeature))
                _addedUnitFeatures.Add(SelectedUnitFeature);
        }

        [RelayCommand]
        public void RemoveSelectedFeature()
        {
            if (SelectedAddedFeatureIndex < 0 || SelectedAddedFeatureIndex >= _addedUnitFeatures.Count)
                return;
            _addedUnitFeatures.RemoveAt(SelectedAddedFeatureIndex);
        }
    }
}
