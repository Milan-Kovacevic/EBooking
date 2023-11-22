using EBooking.WPF.ItemViewModels;
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

namespace EBooking.WPF.Views
{
    /// <summary>
    /// Interaction logic for TripsReservationsView.xaml
    /// </summary>
    public partial class TripsReservationsView : UserControl
    {
        public TripsReservationsView()
        {
            InitializeComponent();
        }

        private void TripReservationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && DataContext is TripReservationsViewModel vm)
            {
                var dataGrid = (DataGrid)sender;
                var selectedRowItem = dataGrid.SelectedItem;

                // Check if an item is selected
                if (selectedRowItem != null && dataGrid.CurrentColumn is not DataGridCheckBoxColumn && dataGrid.CurrentColumn is not DataGridTemplateColumn)
                {
                    var selectedData = (TripReservationItemViewModel)selectedRowItem;
                    vm.ShowReservationDetailsFor(selectedData);
                }
            }
        }
    }
}
