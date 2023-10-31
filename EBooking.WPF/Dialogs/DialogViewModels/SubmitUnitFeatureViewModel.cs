using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Dialogs.DialogViewModels
{
    public partial class SubmitUnitFeatureViewModel : ObservableValidator
    {
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string featureName;

        public IRelayCommand SubmitCommand { get; }

        private readonly Action<SubmitUnitFeatureViewModel> _onSubmitAction;

        public SubmitUnitFeatureViewModel(Action<SubmitUnitFeatureViewModel> onSubmitAction, UnitFeatureItemViewModel? vm = null)
        {
            _onSubmitAction = onSubmitAction;

            featureName = vm?.Name ?? string.Empty;
            SubmitCommand = new RelayCommand(Submit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return FeatureName != string.Empty
                && !HasErrors;
        }

        private void Submit()
        {
            _onSubmitAction(this);
        }
    }
}
