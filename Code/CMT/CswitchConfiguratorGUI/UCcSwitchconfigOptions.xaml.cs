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

namespace CMT.CswitchConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCcSwitchconfigOptions.xaml
    /// </summary>
    public partial class UCcSwitchconfigOptions : UserControl
    {
        private int _val;
        private const int REDUNDANCY = 0;
        private const int MAIN = 1;
        private string[] _BNarray = { "1", "2", "3" };

        public UCcSwitchconfigOptions()
        {
            InitializeComponent();
            _cbBN.ItemsSource = _BNarray;

        }

        private void _rbMain_Checked(object sender, RoutedEventArgs e)
        {
            if(MainWindow.val == null)
                MainWindow.val = new int[2];
            MainWindow.val[0] = MAIN;
            _sp.Visibility = System.Windows.Visibility.Visible;
        }

        private void _rbRed_Checked(object sender, RoutedEventArgs e)
        {
            if (MainWindow.val == null)
                MainWindow.val = new int[2];
            MainWindow.val[0] = REDUNDANCY;
            _sp.Visibility = System.Windows.Visibility.Visible;
        }

        private void _cbBN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = (string)_cbBN.SelectedItem.ToString();
            int.TryParse(str, out MainWindow.val[1]);
            Navigate.SetNextEnable(this, true);
        }
    }
}
