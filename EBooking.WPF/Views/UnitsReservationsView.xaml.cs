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
    /// Interaction logic for UnitsReservationsView.xaml
    /// </summary>
    public partial class UnitsReservationsView : UserControl
    {
        public UnitsReservationsView()
        {
            InitializeComponent();
        }

        private void UnitReservationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var dataGrid = (DataGrid)sender;
                var selectedRowItem = dataGrid.SelectedItem;

                // Check if an item is selected
                if (selectedRowItem != null && dataGrid.CurrentColumn is not DataGridCheckBoxColumn && dataGrid.CurrentColumn is not DataGridTemplateColumn)
                {
                    var selectedData = (UnitReservationItemViewModel)selectedRowItem;
                    if (DataContext is AccommodationReservationsViewModel vm1)
                        vm1.ShowReservationDetailsFor(selectedData);
                    else if (DataContext is UnitReservationsViewModel vm2)
                        vm2.ShowReservationDetailsFor(selectedData);
                }
            }
        }
    }
}
