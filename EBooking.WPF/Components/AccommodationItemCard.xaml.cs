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

namespace EBooking.WPF.Components
{
    /// <summary>
    /// Interaction logic for AccommodationItemCard.xaml
    /// </summary>
    public partial class AccommodationItemCard : UserControl
    {
        public AccommodationItemCard()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is AccommodationItemViewModel vm)
            {
                vm.ShowAccommodationDetails();
            }
        }
    }
}
