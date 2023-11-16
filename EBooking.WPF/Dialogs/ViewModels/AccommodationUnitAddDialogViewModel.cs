using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Dialogs.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.Utility;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class AccommodationUnitAddDialogViewModel : ObservableValidator, IViewModelBase
    {
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
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateAvailableToDateOnAdd))]
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

        public AccommodationUnitAddDialogViewModel(UnitFeaturesStore unitFeaturesStore, AccommodationUnitService accommodationUnitService, DialogHostService dialogHostService)
        {
            _unitFeaturesStore = unitFeaturesStore;
            _accommodationUnitService = accommodationUnitService;
            _dialogHostService = dialogHostService;
            AvailableUnitFeatures = _unitFeaturesStore.UnitFeatures.Select(x => Mapper.Map(x).ToANew<UnitFeatureModel>());
            _addedUnitFeatures = new ObservableCollection<UnitFeatureModel>();
            AddedUnitFeatures = new ListCollectionView(_addedUnitFeatures);
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.ACCOMMODATION_UNIT_ADD_DIALOG_TITLE);
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

            name = string.Empty;
            availableFrom = null;
            availableTo = null;
            numberOfBeds = string.Empty;
            pricePerNight = string.Empty;
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
            await _accommodationUnitService.AddAccommodationUnit(new AccommodationUnit()
            {
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
