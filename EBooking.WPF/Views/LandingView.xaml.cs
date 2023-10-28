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

namespace EBooking.WPF.Views
{
    /// <summary>
    /// Interaction logic for LandingView.xaml
    /// </summary>
    public partial class LandingView : UserControl
    {
        public LandingView()
        {
            InitializeComponent();
        }
        private void RegisterLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DataContext is ViewModels.LandingViewModel vm)
            {
                vm.NavigateToRegister();
            }
        }
        private void LoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DataContext is ViewModels.LandingViewModel vm)
            {
                if (vm.IsLoginEnabled)
                    vm.NavigateToLogin();
            }
        }

        private void UserManualLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DataContext is ViewModels.LandingViewModel vm)
            {
                vm.OpenApplicationManual();
            }
        }
    }
}
