using EBooking.WPF.Dialogs.DialogViewModels;
using EBooking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EBooking.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for SubmitUnitFeatureDialog.xaml
    /// </summary>
    public partial class SubmitUnitFeatureDialog : UserControl
    {
        public SubmitUnitFeatureDialog(Func<SubmitUnitFeatureViewModel, Task> onSubmitAction, UnitFeatureItemViewModel? vm = null)
        {
            InitializeComponent();
            DataContext = new SubmitUnitFeatureViewModel(onSubmitAction, vm);
        }
    }
}
