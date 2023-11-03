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
        public int FeatureId { get; set; }
        [ObservableProperty]
        [Required(ErrorMessage = "!")]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyDataErrorInfo]
        private string featureName;

        public IRelayCommand SubmitCommand { get; }

        private readonly Func<SubmitUnitFeatureViewModel, Task> _onSubmitAction;

        public SubmitUnitFeatureViewModel(Func<SubmitUnitFeatureViewModel, Task> onSubmitAction, UnitFeatureItemViewModel? vm = null)
        {
            _onSubmitAction = onSubmitAction;

            featureName = vm?.Name ?? string.Empty;
            FeatureId = vm?.FeatureId ?? 0;
            SubmitCommand = new AsyncRelayCommand(Submit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return FeatureName != string.Empty
                && !HasErrors;
        }

        private async Task Submit()
        {
            await _onSubmitAction(this);
        }
    }
}
