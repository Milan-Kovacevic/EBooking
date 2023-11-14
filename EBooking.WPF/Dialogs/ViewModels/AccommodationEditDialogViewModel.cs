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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class AccommodationEditDialogViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private string dialogTitle;
        public int AccommodationId { get; set; }

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string name;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private AccommodationTypeModel? type;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private LocationModel? location;

        [ObservableProperty]
        [CustomValidation(typeof(Validators), nameof(Validators.ValidateRequiredProperty))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string address;

        public IRelayCommand SubmitCommand { get; }
        public IEnumerable<AccommodationTypeModel> AccommodationTypes { get; }
        public IEnumerable<LocationModel> Locations { get; }

        private readonly LocationsStore _locationsStore;
        private readonly UserStore _userStore;
        private readonly AccommodationService _accommodationService;
        private readonly DialogHostService _dialogHostService;

        public AccommodationEditDialogViewModel(LocationsStore locationsStore, UserStore userStore, AccommodationService accommodationService, DialogHostService dialogHostService)
        {
            _locationsStore = locationsStore;
            _userStore = userStore;
            _accommodationService = accommodationService;
            _dialogHostService = dialogHostService;

            Locations = _locationsStore.Locations.Select(x => Mapper.Map(x).ToANew<LocationModel>());
            AccommodationTypes = new List<AccommodationTypeModel>()
            {
                new AccommodationTypeModel(AccommodationType.APARTMENT),
                 new AccommodationTypeModel(AccommodationType.HOTEL),
            };
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
            dialogTitle = LanguageTranslator.Translate(LanguageTranslator.MessageType.ACCOMMODATION_EDIT_DIALOG_TITLE);
            var accommodation = accommodationService.GetSelectedAccommodation();
            name = accommodation?.Name ?? string.Empty;
            type = new AccommodationTypeModel(accommodation?.Type ?? AccommodationType.APARTMENT);
            location = Mapper.Map(accommodation?.Location).ToANew<LocationModel>();
            address = accommodation?.Address ?? string.Empty;
            AccommodationId = accommodation?.AccommodationId ?? 0;
        }

        private bool CanSubmit()
        {
            return Name != string.Empty
                && Address != string.Empty
                && Type != null
                && Location != null
                && !HasErrors;
        }

        private async Task Submit()
        {
            await _accommodationService.UpdateAccommodation(new Accommodation()
            {
                Name = Name,
                Type = Type?.Type ?? AccommodationType.APARTMENT,
                Address = Address,
                LocationId = Location?.LocationId ?? 0,
                UserId = _userStore.CurrentUser?.UserId ?? 0,
                AccommodationId = AccommodationId
            });
            _dialogHostService.CloseDialogHost();
        }
    }
}
