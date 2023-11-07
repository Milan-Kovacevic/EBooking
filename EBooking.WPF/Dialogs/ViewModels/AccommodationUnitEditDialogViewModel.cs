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
using System.Xml.Linq;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class AccommodationUnitEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        public int UnitId { get; set; }
        [ObservableProperty]
        private string dialogTitle;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string name;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyPropertyChangedFor(nameof(AvailableTo))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateAvailableFromDate))]
        [NotifyDataErrorInfo]
        private DateTime? availableFrom;
        partial void OnAvailableFromChanged(DateTime? value)
        {
            ValidateProperty(AvailableTo, nameof(AvailableTo));
        }

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyPropertyChangedFor(nameof(AvailableFrom))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateAvailableToDateOnAdd))]
        [NotifyDataErrorInfo]
        private DateTime? availableTo;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateNumberOfBeds))]
        private string numberOfBeds;

        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidatePricePerNight))]
        [NotifyDataErrorInfo]
        private string pricePerNight;

        [ObservableProperty]
        private UnitFeatureModel? selectedFeature;
        [ObservableProperty]
        private int selectedAddedFeatureIndex;

        public IRelayCommand SubmitCommand { get; }
        public IEnumerable<UnitFeatureModel> UnitFeatures { get; }
        public ObservableCollection<UnitFeatureModel> SelectedFeatures { get; }

        private readonly UnitFeaturesStore _unitFeaturesStore;
        private readonly AccommodationUnitService _accommodationUnitService;
        private readonly DialogHostService _dialogHostService;


        public AccommodationUnitEditDialogViewModel(UnitFeaturesStore unitFeaturesStore, AccommodationUnitService accommodationUnitService, DialogHostService dialogHostService)
        {
            _unitFeaturesStore = unitFeaturesStore;
            _accommodationUnitService = accommodationUnitService;
            _dialogHostService = dialogHostService;
            UnitFeatures = _unitFeaturesStore.UnitFeatures.Select(x => Mapper.Map(x).ToANew<UnitFeatureModel>());
            dialogTitle = "Edit Accommodation Unit";
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            var accommodationUnit = accommodationUnitService.GetSelectedAccommodationUnit();
            name = accommodationUnit?.Name ?? string.Empty;
            availableFrom = accommodationUnit?.AvailableFrom ?? null;
            availableTo = accommodationUnit?.AvailableTo ?? null;
            numberOfBeds = accommodationUnit?.NumberOfBeds.ToString() ?? string.Empty;
            pricePerNight = accommodationUnit?.PricePerNight.ToString() ?? string.Empty;
            UnitId = accommodationUnit?.UnitId ?? 0;
            SelectedFeatures = new ObservableCollection<UnitFeatureModel>(accommodationUnit?.Features.Select(x => Mapper.Map(x).ToANew<UnitFeatureModel>()) ?? Array.Empty<UnitFeatureModel>());
            selectedFeature = null;
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
            var features = new List<UnitFeature>(SelectedFeatures.Select(x => Mapper.Map(x).ToANew<UnitFeature>()));
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
            if (SelectedFeature != null && !SelectedFeatures.Contains(SelectedFeature))
                SelectedFeatures.Add(SelectedFeature);
        }

        [RelayCommand]
        public void RemoveSelectedFeature()
        {
            if (SelectedAddedFeatureIndex < 0 || SelectedAddedFeatureIndex >= SelectedFeatures.Count)
                return;
            SelectedFeatures.RemoveAt(SelectedAddedFeatureIndex);
        }
    }
}
