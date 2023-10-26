﻿using System;
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
    /// Interaction logic for ConfirmExitDialog.xaml
    /// </summary>
    public partial class ConfirmExitDialog : UserControl
    {
        public ConfirmExitDialog()
        {
            InitializeComponent();
        }

        private void ConfirmExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}