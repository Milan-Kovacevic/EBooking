using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.Domain.DTOs;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.DialogViewModels
{
    public partial class SubmitLocationViewModel : ObservableValidator
    {
        public int LocationId { get; set; }
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string countryName;
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string cityName;

        public IRelayCommand SubmitCommand { get; }

        private readonly Func<SubmitLocationViewModel, Task> _onSubmitAction;
        public SubmitLocationViewModel(Func<SubmitLocationViewModel, Task> onSubmitAction, LocationItemViewModel? vm = null)
        {
            _onSubmitAction = onSubmitAction;

            countryName = vm?.Country ?? string.Empty;
            cityName = vm?.City ?? string.Empty;
            LocationId = vm?.LocationId ?? 0;
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return CountryName != string.Empty
                && CityName != string.Empty &&
                !HasErrors;
        }

        private async Task Submit()
        {
            await _onSubmitAction(this);
        }
    }
}
