using AgileObjects.AgileMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.Domain.Enums;
using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.DialogViewModels
{
    public partial class SubmitAccommodationViewModel : ObservableValidator
    {
        public int AccommodationId { get; set; }
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

        public record class LocationModel
        {
            public int LocationId { get; set; }
            public required string Country { get; set; }
            public required string City { get; set; }
            public override string ToString() => $"{Country}, {City}";
        }

        public record class AccommodationTypeModel(AccommodationType Type)
        {
            public string TypeName { get => Type.ToString().ToLowerInvariant(); }
            public override string ToString() => $"{TypeName}";
        }

        public IRelayCommand SubmitCommand { get; }
        public IEnumerable<AccommodationTypeModel> AccommodationTypes { get; }
        public IEnumerable<LocationModel> Locations { get; }

        private readonly Func<SubmitAccommodationViewModel, Task> _onSubmitAction;
        private readonly LocationsStore _locationsStore;

        public SubmitAccommodationViewModel(LocationsStore locationsStore, Func<SubmitAccommodationViewModel, Task> onSubmitAction)
        {
            _locationsStore = locationsStore;
            _onSubmitAction = onSubmitAction;
            Locations = _locationsStore.Locations.Select(x => Mapper.Map(x).ToANew<LocationModel>());
            AccommodationTypes = new List<AccommodationTypeModel>()
            {
                new AccommodationTypeModel(AccommodationType.APARTMENT),
                 new AccommodationTypeModel(AccommodationType.HOTEL),
            };
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);

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
            await _onSubmitAction(this);
        }
    }
}
