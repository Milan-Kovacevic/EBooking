using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Dialogs.Models;
using EBooking.WPF.Services;
using EBooking.WPF.Stores;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.ViewModels
{
    public partial class AccommodationAddDialogViewModel : ObservableValidator, IViewModelBase
    {
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
        [NotifyDataErrorInfo]
        private AccommodationTypeModel? type;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private LocationModel? location;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
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

        public AccommodationAddDialogViewModel(LocationsStore locationsStore, UserStore userStore, AccommodationService accommodationService, DialogHostService dialogHostService)
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
            dialogTitle = "Create New Accommodation";
            name = string.Empty;
            type = null;
            location = null;
            address = string.Empty;
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
            await _accommodationService.AddAccommodation(new Accommodation()
            {
                Name = Name,
                Type = Type?.Type ?? AccommodationType.APARTMENT,
                Address = Address,
                LocationId = Location?.LocationId ?? 0,
                UserId = _userStore.CurrentUser?.UserId ?? 0
            });
            _dialogHostService.CloseDialogHost();
        }
    }
}
