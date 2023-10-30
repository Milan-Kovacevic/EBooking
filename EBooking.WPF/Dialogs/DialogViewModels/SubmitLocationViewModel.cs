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

        private readonly Action<SubmitLocationViewModel> _onSubmitAction;
        public SubmitLocationViewModel(Action<SubmitLocationViewModel> onSubmitAction, LocationItemViewModel? vm = null)
        {
            _onSubmitAction = onSubmitAction;

            countryName = vm?.Country ?? string.Empty;
            cityName = vm?.City ?? string.Empty;
            SubmitCommand = new RelayCommand(Submit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return CountryName != string.Empty
                && CityName != string.Empty &&
                !HasErrors;
        }

        private void Submit()
        {
            _onSubmitAction(this);
        }
    }
}
