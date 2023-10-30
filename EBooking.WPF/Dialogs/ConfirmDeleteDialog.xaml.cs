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
    /// Interaction logic for ConfirmDeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDeleteDialog : UserControl
    {
        public ICommand OnYesCommand
        {
            get { return (ICommand)GetValue(OnYesCommandProperty); }
            set { SetValue(OnYesCommandProperty, value); }
        }

        public static readonly DependencyProperty OnYesCommandProperty =
            DependencyProperty.Register("OnYesCommand", typeof(ICommand), typeof(ConfirmDeleteDialog), new PropertyMetadata(null));


        public ConfirmDeleteDialog()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
